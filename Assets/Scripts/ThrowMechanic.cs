using System;
using System.Collections;
using Scripts.Extensions;
using TriInspector;
using UnityEditor.EditorTools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ThrowMechanic : MonoBehaviour
{
	[SerializeField, Required] private Rigidbody ballPrefab;
	private Vector2 _throwValue;
	private Rigidbody ball;
	[SerializeField] private float fullThrowForce = 3f;
	[SerializeField] private float fullThrowVerticalForce = 2f;

	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
		if (!ball)
			SpawnNewBall();
	}
	
	public void OnThrow(Vector2 value)
	{
		if (!ball) return;
		
		_throwValue = value;
		StartCoroutine(Throw());
	}

	public void OnValueChanged(Vector2 value)
	{
		_throwValue = value;
	}

	private IEnumerator Throw()
	{
		ball.isKinematic = false;
		ball.AddForce(GetThrowForce());
		ball = null;
		yield return new WaitForSeconds(1f);

		SpawnNewBall();
	}

	public Vector3 GetThrowForce()
	{
		return new(
			-_throwValue.x * fullThrowForce,
			_throwValue.magnitude * fullThrowVerticalForce,
			-_throwValue.y * fullThrowForce
			);
	}

	private void SpawnNewBall()
	{
		ball = Instantiate(ballPrefab);
		ball.transform.position = transform.position;
		ball.isKinematic = true;
	}
}