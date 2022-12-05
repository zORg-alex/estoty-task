using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ISphereCoordinatesUser {
	new Rigidbody rigidbody;
	public float movementMultiplier = 1f;
	private float turningMultiplier = 1f;
	public Transform MoveAlongTarget;
	public float lerpSmoothingTurn = .05f;

	public SphereCoordinates SphereCoordinates { get; set; }

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Move(Vector2 input)
	{
		Vector3 moveVector = new Vector3(input.x, 0, input.y).normalized * movementMultiplier;
		if (MoveAlongTarget)
		{
			moveVector = SphereCoordinates.GetForwardHorizontalRotation(transform.position, MoveAlongTarget.forward) * moveVector;
			if (moveVector.sqrMagnitude > 0)
			{
				Quaternion rotation = Quaternion.LookRotation(moveVector, transform.up);
				transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lerpSmoothingTurn);
			}
		}
		else
			moveVector = transform.TransformVector(moveVector);
		
		rigidbody.MovePosition(transform.position + moveVector);
	}
	public void Turn(float input)
	{
		if (!MoveAlongTarget)
			rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(0, input * turningMultiplier, 0));
	}
}
