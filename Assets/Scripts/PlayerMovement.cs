using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IMove, ISphereCoordinatesUser {
	public float movementMultiplier;
	new Rigidbody rigidbody;

	public SphereCoordinates SphereCoordinates { get; set; }

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Move(Vector2 input)
	{
		Vector3 newPos = transform.localPosition + new Vector3(input.x * movementMultiplier, 0, input.y * movementMultiplier);
		rigidbody.Move(newPos, SphereCoordinates.GetForwardHorizontalRotation(newPos, transform.forward));
	}
}
