using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class CharacterAnimation : MonoBehaviour
{
	private Vector3 oldForward = Vector3.forward;
	Quaternion oldRotation;
	Vector3 oldPosition;
	public bool isMoving;
	private float turnValue;
	private PlayerMovement pm;
	public Animator Animator;
	private int SpeedHash;
	private int TurnHash;
	public float turnAbsThreshold = 1;
	public float moveMax = 1.5f;

	private void OnEnable()
	{
		pm = GetComponent<PlayerMovement>();
		SpeedHash = Animator.StringToHash("Speed");
		TurnHash = Animator.StringToHash("Turn");
	}

	public void Update()
	{
		var targetDirection = pm.TargetDirection;
		var fdot = Mathf.Clamp01( Vector3.Dot(targetDirection, transform.forward));
		var rdot = Vector3.Dot(targetDirection, transform.right);
		Animator.SetFloat(SpeedHash, fdot * moveMax);
		Animator.SetFloat(TurnHash, rdot * turnAbsThreshold);

	}
}
