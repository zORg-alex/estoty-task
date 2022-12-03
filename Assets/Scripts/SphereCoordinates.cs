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

	private void Start()
	{
		InitUsers();
	}

	private void InitUsers()
	{
		foreach (var u in GetComponentsInChildren<ISphereCoordinatesUser>())
		{
			u.SphereCoordinates = this;
		}
	}

	public Vector3 GetUp(Transform t) => GetUpRaw(t).normalized;
	public Vector3 GetUpRaw(Transform t) => t.position - transform.position;
	public Vector3 GetUp(Vector3 position) => GetUpRaw(position).normalized;
	public Vector3 GetUpRaw(Vector3 position) => position - transform.position;

	public Vector3 GetDown(Transform t) => GetDownRaw(t).normalized;
	public Vector3 GetDownRaw(Transform t) => transform.position - t.position;

	public Quaternion GetForwardHorizontalRotation(Transform t) => Quaternion.LookRotation(GetUpRaw(t), t.forward) * lookRotationfix;
	public Quaternion GetForwardHorizontalRotation(Vector3 position, Vector3 forward) => Quaternion.LookRotation(GetUpRaw(position), forward) * lookRotationfix;
}
