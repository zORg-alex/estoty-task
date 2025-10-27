using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
	private IMaterialTween _tween;
	[SerializeField]
	private UnityEventInt onBallScored = new ();
	public event UnityAction<int> OnBallScored { add => onBallScored.AddListener(value); remove => onBallScored.RemoveListener(value); }
	[SerializeField]
	private UnityEventInt onFail = new ();
	public event UnityAction<int> OnFail { add => onFail.AddListener(value); remove => onFail.RemoveListener(value); }

	[SerializeField] private Transform _scoreEffectPrefab;
	[SerializeField] private Transform _appearEffectPrefab;
	[SerializeField] private Transform _failEffectPrefab;
	
	[SerializeField] private Color goodColor = Color.cornflowerBlue;
	[SerializeField] private Color badColor = Color.brown;
	
	private bool _isGood = true;
	private bool _isTweening = false;
	public bool isGood => _isGood;
	public bool isTweening => _isTweening;
	private int _id;

	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize() => InitializeAsync().Forget();

	private async UniTaskVoid InitializeAsync()
	{

#if UNITY_EDITOR
		onFail.RemoveAllListeners();
		onBallScored.RemoveAllListeners();
#endif
		await UniTask.Yield();
		TryGetComponent(out _tween);
		_tween.SetColor(goodColor);
	}

	public void OnBallDetected(Collider other)
	{
		if (_isGood)
		{
			if (_scoreEffectPrefab)
				Instantiate(_scoreEffectPrefab, transform.position, Quaternion.identity);
			onBallScored.Invoke(_id);
		}
		else
		{
			if (_failEffectPrefab)
				Instantiate(_failEffectPrefab, transform.position, Quaternion.identity);
			onFail.Invoke(_id);
		}
	}
	
	public void Flip()
	{
		if (_tween == null) Debug.LogError("_tween not found", this);
		_tween?.StartTweenTo(_isGood ? badColor : goodColor);
		_isTweening = true;
	}

	public void FlipFinished()
	{
		_isGood = !_isGood;
		_isTweening = false;
	}
	
	public void SetID(int id) => _id = id;

	public void JustMoved()
	{
		if (_appearEffectPrefab)
			Instantiate(_appearEffectPrefab, transform.position, Quaternion.identity);
	}
}