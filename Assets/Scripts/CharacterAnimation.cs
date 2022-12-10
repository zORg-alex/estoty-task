using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class CharacterAnimation : MonoBehaviour
{
	private PlayerMovement pm;
	public Animator Animator;
	private int SpeedHash;
	private int TurnHash;
	public float TurnAbsThreshold = 1;
	public float MoveMax = 1.5f;
	private float fdot;
	private float rdot;
	public float LerpSmoothing = .05f;

	private void OnEnable()
	{
		pm = GetComponent<PlayerMovement>();
		SpeedHash = Animator.StringToHash("Speed");
		TurnHash = Animator.StringToHash("Turn");
	}

	public void Update()
	{
		var targetDirection = pm.TargetDirection;

		float newFDot = Mathf.Clamp01(Vector3.Dot(targetDirection, transform.forward));
		fdot = Mathf.Abs(fdot - newFDot) > .01f ? Mathf.Lerp(fdot, newFDot, LerpSmoothing) : newFDot;
		Animator.SetFloat(SpeedHash, fdot * MoveMax);

		rdot = Vector3.Dot(targetDirection, transform.right);
		Animator.SetFloat(TurnHash, rdot * TurnAbsThreshold);

	}
}
