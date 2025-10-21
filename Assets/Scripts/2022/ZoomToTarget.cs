using UnityEngine;

public class ZoomToTarget : MonoBehaviour
{
	public Transform Target;
	public float ZoomValue;
	public float ZoomMultiplier = .1f;
	public float MinZoom = 1f;
	public float MaxZoom = 10f;
	public float LerpSmoothing = .05f;
	public Vector3 InitialRelativePosition;

	[ContextMenu("SetInitialPosition")]
	void SetInitialPosition()
	{
		InitialRelativePosition = transform.InverseTransformVector(transform.position - Target.position).normalized;
	}
	[ContextMenu("ResetPosition")]
	void ResetPosition()
	{
		transform.position = Target.position + transform.TransformVector(InitialRelativePosition);
		ZoomValue = InitialRelativePosition.magnitude;
	}

	public void Zoom(float zoomDelta)
	{
		ZoomValue = Mathf.Clamp(ZoomValue + zoomDelta * ZoomMultiplier, MinZoom, MaxZoom);
	}
	private void Update()
	{
		if (Target)
		{
			var zoom = Mathf.Lerp((transform.position - Target.position).magnitude, ZoomValue, LerpSmoothing);
			transform.position = Vector3.Lerp(transform.position, Target.position + transform.TransformVector(InitialRelativePosition * zoom), LerpSmoothing);
		}
	}
}