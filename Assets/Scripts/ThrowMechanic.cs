using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Extensions;
using Scripts.UI;
using TriInspector;
using UnityEngine;

public class ThrowMechanic : MonoBehaviour
{
	[SerializeField, Required] private Rigidbody ballPrefab;
	private Vector2 _throwValue;
	private Rigidbody ball;
	private List<GameObject> balls = new();
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
		AddBall(ball.gameObject);
		ball = null;
		yield return new WaitForSeconds(1f);

		SpawnNewBall();
	}

	private void AddBall(GameObject gameObject)
	{
		for (int i = 0; i < balls.Count; i++)
		{
			if (!balls[i])
			{
				balls[i] = gameObject;
				return;
			}
		}
		balls.Add(gameObject);
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

	public void ResetBalls()
	{
		foreach (var b in balls)
		{
			Destroy(b);
		}
		balls.Clear();
	}
}