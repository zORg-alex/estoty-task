using System;
using PrimeTween;
using Scripts.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MaterialTween : MonoBehaviour
{
    private static readonly int ColorProp = Shader.PropertyToID("_BaseColor");

    [SerializeField] protected TweenSettings settings;
    [SerializeField] private new Renderer renderer;
    [SerializeField] protected Color currentColor;
    
    [SerializeField]
    private UnityEvent onTweenFinished = new ();
    public event UnityAction OnTweenFinished { add => onTweenFinished.AddListener(value); remove => onTweenFinished.RemoveListener(value); }

    
    private MaterialPropertyBlock _mpb;

    private void Start() => Initialize();
    private void OnEnable() => this.OnAssemblyReload(Initialize);

    private void Initialize()
    {
        if (!TryGetComponent(out renderer)) { Debug.LogError("renderer is null", this); return; }
        
        _mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(_mpb, 0);
        SetColor(currentColor);
    }

    virtual public void StartTweeningTo(Color color)
    {
        TweenPropertyBlock(currentColor, color);
    }

    protected Tween TweenPropertyBlock(Color start, Color end) =>
        Tween.Custom(start, end, settings, SetColor).OnComplete(OnAfterTweening);

    public void SetColor(Color color)
    {
        _mpb.SetColor(ColorProp, color);
        renderer.SetPropertyBlock(_mpb, 0);
    }

    protected void OnAfterTweening()
    {
         onTweenFinished.Invoke();
         currentColor = _mpb.GetColor(ColorProp);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MaterialTween))]
    private class MaterialTweenInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Tween to blue"))
                {
                    (target as MaterialTween)?.StartTweeningTo(Color.blue);
                }
                if (GUILayout.Button("Tween to red"))
                {
                    (target as MaterialTween)?.StartTweeningTo(Color.red);
                }
            }
        }
    }
#endif
}