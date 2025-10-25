using System;
using System.Collections;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[DeclareFoldoutGroup("ev", Title = "Events")]
public class ThrowAction : MonoBehaviour
{
    [Serializable]
    public class ThrowEvent : UnityEvent<Vector2> { }
    
    [SerializeField, Required] private InputActionReference throwStick;
    [SerializeField] private float engageThreshold = 0.1f; // magnitude considered "pressed"
    [SerializeField] private float zeroSpeedThreshold = .5f; // speed (units/sec) toward zero considered "fast" (onscreen)
    [Group("ev"), SerializeField] private ThrowEvent onThrow = new ();
    public event UnityAction<Vector2> OnThrow { add => onThrow.AddListener(value); remove => onThrow.RemoveListener(value); }
    [Group("ev"), SerializeField]
    private ThrowEvent onValueChanged = new ();
    public event UnityAction<Vector2> OnValueChanged { add => onValueChanged.AddListener(value); remove => onValueChanged.RemoveListener(value); }

    
    private void Start() => Initialize();
    private void Initialize()
    {
        throwStick?.action.Enable();
    }
    private void OnEnable()
    {
        this.OnAssemblyReload(Initialize);

        StartCoroutine(UpdateCR());
    }

    private IEnumerator UpdateCR()
    {
        var apogee = Vector2.zero;
        var lastValue = Vector2.zero;
        while (enabled)
        {
            var stickValue = throwStick.action.ReadValue<Vector2>();
            var stickDelta = stickValue - lastValue;
            var speedToOrigin = 0f;
            if (lastValue.sqrMagnitude > 1e-6f)
                speedToOrigin = Vector2.Dot(stickDelta, -lastValue.normalized);

            if (stickValue.magnitude > engageThreshold)
            {
                if (speedToOrigin < zeroSpeedThreshold)
                    apogee = stickValue;
            }

            if (apogee.sqrMagnitude > stickValue.sqrMagnitude && stickValue.sqrMagnitude < .05f)
            {
                Throw(apogee);
                apogee = Vector2.zero;
            }
            else
            {
                UpdateValue(stickValue);
            }
            
            lastValue = stickValue;
            yield return null;
        }
    }

    private void UpdateValue(Vector2 value)
    {
        onValueChanged.Invoke(value);
    }

    private void Throw(Vector2 value)
    {
        onThrow.Invoke(value);
    }
}