using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
	private Vector3 oldForward = Vector3.forward;
	Quaternion oldRotation;
	public bool isMoving;
	private float turnValue;

	public void Move(Vector2 moveInput)
	{
		isMoving = moveInput.sqrMagnitude > 0f;
		turnValue = Vector3.Dot(oldForward, transform.forward);

		oldForward = transform.forward;
		oldRotation = transform.rotation;
	}
}
