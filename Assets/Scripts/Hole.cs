using System;
using System.Collections;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
	private MaterialTween _tween;
	[SerializeField] private float delayToBallDestruction = 2f;
	[SerializeField]
	private UnityEventInt onBallScored = new ();
	public event UnityAction<int> OnBallScored { add => onBallScored.AddListener(value); remove => onBallScored.RemoveListener(value); }
	[SerializeField]
	private UnityEventInt onFail = new ();
	public event UnityAction<int> OnFail { add => onFail.AddListener(value); remove => onFail.RemoveListener(value); }

	[SerializeField] private Color goodColor = Color.cornflowerBlue;
	[SerializeField] private Color badColor = Color.brown;
	
	private bool _isGood = true;
	public bool isGood => _isGood;
	private int _id;

	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
#if UNITY_EDITOR
		onFail.RemoveAllListeners();
		onBallScored.RemoveAllListeners();
#endif
		TryGetComponent(out _tween);
		_tween.SetColor(goodColor);
	}

	public void OnBallDetected(Collider other)
	{
		if (_isGood)
			onBallScored.Invoke(_id);
		else
			onFail.Invoke(_id);
	}
	
	public void Flip()
	{
		if (!_tween) Debug.LogError("_tween not found", this);
		_tween.StartTweeningTo(_isGood ? badColor : goodColor);
	}

	public void FlipFinished()
	{
		_isGood = !_isGood;
	}
	
	public void SetID(int id) => _id = id;
}