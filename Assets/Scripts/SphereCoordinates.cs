using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCoordinates : MonoBehaviour
{
	public float Radius = 1f;
	private Matrix4x4 worldToLocalMatrix;
	private Quaternion lookRotationfix;

	private void OnEnable()
	{
		worldToLocalMatrix = transform.worldToLocalMatrix;
		lookRotationfix = Quaternion.Euler(90, 0, 0);
	}

	public Vector3 GetUp(Transform t) => -GetDownRaw(t).normalized;
	public Vector3 GetUpRaw(Transform t) => t.position - transform.position;

	public Vector3 GetDown(Transform t) => GetDownRaw(t).normalized;
	public Vector3 GetDownRaw(Transform t) => transform.position - t.position;

	public Quaternion GetForwardHorizontalRotation(Transform t) => Quaternion.LookRotation(GetUpRaw(t), t.forward) * lookRotationfix;
}
