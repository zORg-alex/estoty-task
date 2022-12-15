using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ISphereCoordinatesUser {
	new Rigidbody rigidbody;
	public Transform MoveAlongTarget;
	public float MovementMultiplier = 1f;
	public float LerpSmoothing = .05f;
	private Vector3 moveVector;
	private Vector3 allignedMoveVector;

	public SphereCoordinates SphereCoordinates { get; set; }

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Move(Vector2 input)
	{
		if (!MoveAlongTarget) { Debug.LogError("MoveAlongTarget missing"); return; }
		//Convert input vector
		var m = new Vector3(input.x, 0, input.y).normalized * MovementMultiplier;
		//Smooth it
		moveVector = Vector3.Lerp(moveVector, m, LerpSmoothing);

		allignedMoveVector = SphereCoordinates.GetForwardHorizontalRotation(transform.position, MoveAlongTarget.forward) * moveVector;
		if (allignedMoveVector.sqrMagnitude > .00001f)
		{
			Quaternion rotation = Quaternion.LookRotation(allignedMoveVector, transform.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LerpSmoothing);
			rigidbody.MovePosition(transform.position + allignedMoveVector * Time.deltaTime);
		}
	}

	public Vector3 TargetDirection => allignedMoveVector.normalized;
}
