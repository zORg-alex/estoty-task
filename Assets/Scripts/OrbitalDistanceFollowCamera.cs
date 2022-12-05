using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OrbitalDistanceFollowCamera : MonoBehaviour, ISphereCoordinatesUser
{
	public Transform target;
	public SphereCoordinates SphereCoordinates { get; set; }
	public float distanceAngle = 15f;
	public float lerpSmoothing = .05f;
	public float turnValue = 0;
	public float turnMultiplier = .1f;

	private void Update()
	{
		Vector3 sphereCenterToTarget = target.position - SphereCoordinates.transform.position;
		Vector3 sphereCenterToThis = transform.position - SphereCoordinates.transform.position;
		Vector3 turnOffset = transform.TransformVector(Vector3.right * (turnValue * turnMultiplier));
		var currentRotation = Quaternion.FromToRotation(sphereCenterToTarget, sphereCenterToThis + turnOffset);
		currentRotation.ToAngleAxis(out _, out var axis);
		var ralativeRotation = Quaternion.AngleAxis(distanceAngle, axis);

		var adjustedPosition = SphereCoordinates.transform.position + ralativeRotation * sphereCenterToTarget;
		var adjustedRotation = SphereCoordinates.GetForwardHorizontalRotation(adjustedPosition, target.position - adjustedPosition);
		transform.position = Vector3.Lerp(transform.position, adjustedPosition, lerpSmoothing);
		transform.rotation = Quaternion.Lerp(transform.rotation, adjustedRotation, lerpSmoothing);
	}

	public void Turn(float turn)
	{
		turnValue = Mathf.Lerp(turnValue, turn, lerpSmoothing);
	}

#if UNITY_EDITOR
	[ContextMenu("Update")]
	private void EditorUpdate()
	{
		var lerpVal = lerpSmoothing;
		lerpSmoothing = 1;
		Update();
		lerpSmoothing = lerpVal;
	}
#endif
}
