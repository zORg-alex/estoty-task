using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class CharacterAnimation : MonoBehaviour
{
	private PlayerMovement pm;
	public Animator Animator;
	private static readonly int speedHash = Animator.StringToHash("Speed");
	private static readonly int turnHash = Animator.StringToHash("Turn");
	public float TurnAbsThreshold = 1;
	public float MoveMax = 1.5f;
	public float LerpSmoothing = .05f;
	private float fdot;
	private float rdot;
	private Vector3 lastPos;
	public float maxSpeed;

	private void OnEnable()
	{
		pm = GetComponent<PlayerMovement>();
		lastPos = transform.position;
	}

	public void Update()
	{
		var targetDirection = pm.TargetDirection;

		var normalizedDifference = (transform.position - lastPos).magnitude / maxSpeed;
		float newFDot = Mathf.Clamp01(Vector3.Dot(targetDirection, transform.forward));
		fdot = Mathf.Abs(fdot - newFDot) > .01f ? Mathf.Lerp(fdot, newFDot * normalizedDifference, LerpSmoothing) : newFDot;
		Animator.SetFloat(speedHash, fdot * MoveMax);

		rdot = Vector3.Dot(targetDirection, transform.right);
		Animator.SetFloat(turnHash, rdot * TurnAbsThreshold);

		lastPos = transform.position;
	}
}
