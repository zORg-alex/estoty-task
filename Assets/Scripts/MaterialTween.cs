using System;
using System.Collections.Generic;
using PrimeTween;
using Scripts.Extensions;
using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public class MaterialTween : MonoBehaviour, IMaterialTween
{
    private static readonly int ColorProp = Shader.PropertyToID("_BaseColor");
    public enum Property
    {
        BaseColor=560,
        Color = 181,
        EmissionColor = 69,
        SpecColor = 443,
    }

    // public List<Prop> PropList;
    // [Serializable]
    // public class Prop
    // {
    //     public string str;
    //     public int id;
    //     
    // }
    // [Button]
    // public void UpdateProps()
    // {
    //     for (int i = 0; i < PropList.Count; i++)
    //     {
    //         PropList[i].id = Shader.PropertyToID(PropList[i].str);
    //     }
    // }

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

    public virtual void StartTweenTo(Color color)
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

    public void SetMaterialProperty(Color color, Property property)
    {
        _mpb.SetColor((int)property, color);
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
                    (target as MaterialTween)?.StartTweenTo(Color.blue);
                }
                if (GUILayout.Button("Tween to red"))
                {
                    (target as MaterialTween)?.StartTweenTo(Color.red);
                }
            }
        }
    }
#endif
}

public interface IMaterialTween
{
    public void StartTweenTo(Color color);
    public void SetColor(Color color);
}