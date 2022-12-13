using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OrbitalDistanceFollowCamera : MonoBehaviour, ISphereCoordinatesUser
{
	public Transform Target;
	public SphereCoordinates SphereCoordinates { get; set; }
	public float DistanceAngle = 15f;
	public float LerpSmoothing = .05f;
	public float TurnValue = 0;
	public float TurnMultiplier = .1f;

	private void Update()
	{
		Vector3 sphereCenterToTarget = Target.position - SphereCoordinates.transform.position;
		Vector3 sphereCenterToThis = transform.position - SphereCoordinates.transform.position;
		Vector3 turnOffset = transform.TransformVector(Vector3.right * (TurnValue * TurnMultiplier));
		var currentRotation = Quaternion.FromToRotation(sphereCenterToTarget, sphereCenterToThis + turnOffset);
		currentRotation.ToAngleAxis(out _, out var axis);
		var ralativeRotation = Quaternion.AngleAxis(DistanceAngle, axis);

		var adjustedPosition = SphereCoordinates.transform.position + ralativeRotation * sphereCenterToTarget;
		var adjustedRotation = SphereCoordinates.GetForwardHorizontalRotation(adjustedPosition, Target.position - adjustedPosition);
		transform.position = Vector3.Lerp(transform.position, adjustedPosition, LerpSmoothing);
		transform.rotation = Quaternion.Lerp(transform.rotation, adjustedRotation, LerpSmoothing);
	}

	public void Turn(float turn)
	{
		TurnValue = Mathf.Lerp(TurnValue, turn, LerpSmoothing);
	}

#if UNITY_EDITOR
	[ContextMenu("Update")]
	private void EditorUpdate()
	{
		var lerpVal = LerpSmoothing;
		LerpSmoothing = 1;
		Update();
		LerpSmoothing = lerpVal;
	}
#endif
}
