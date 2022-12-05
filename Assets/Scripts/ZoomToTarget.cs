using UnityEngine;

public class ZoomToTarget : MonoBehaviour
{
	public Transform target;
	public float zoomValue;
	public float zoomMultiplier = .1f;
	public float minZoom = 1f;
	public float maxZoom = 10f;
	public float lerpSmoothing = .05f;
	public Vector3 initialRelativePosition;

	[ContextMenu("SetInitialPosition")]
	void SetInitialPosition()
	{
		initialRelativePosition = transform.InverseTransformVector(transform.position - target.position).normalized;
	}
	[ContextMenu("ResetPosition")]
	void ResetPosition()
	{
		transform.position = target.position + transform.TransformVector(initialRelativePosition);
		zoomValue = initialRelativePosition.magnitude;
	}

	public void Zoom(float zoomDelta)
	{
		zoomValue = Mathf.Clamp(zoomValue + zoomDelta * zoomMultiplier, minZoom, maxZoom);
		if (target)
		{
			var zoom = Mathf.Lerp((transform.position - target.position).magnitude, zoomValue, lerpSmoothing);
			transform.position = target.position + transform.TransformVector(initialRelativePosition * zoom);

		}
	}
}