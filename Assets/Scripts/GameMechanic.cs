using System;
using System.Collections;
using System.Linq;
using Scripts.Audio;
using Scripts.Extensions;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[DeclareFoldoutGroup("ev", Title = "Events")]
public class GameMechanic : MonoBehaviour
{
	[SerializeField, Required] private HexGrid grid;

	[SerializeField] private float baseGameTime = 30f;
	[SerializeField] private float holeFlipTime = 5f;
	[SerializeField] private float timerIncrement = 5f;
	[SerializeField] private float timerDecrement = 10f;
	[SerializeField] private float musicTransitionThreshold = 5f;
	[SerializeField] private float[] musicIntensityTimerThreshold = new[] { 45f, 30f, 15f };
	[SerializeField] private AudioClip[] musicByIntensity;
	private int _currentIntensity;
	private float _lastIntensityChange;
	
	[Group("ev"), SerializeField]
	private UnityEventInt onScoreChanged = new ();
	public event UnityAction<int> OnScoreChanged { add => onScoreChanged.AddListener(value); remove => onScoreChanged.RemoveListener(value); }
	[Group("ev"), SerializeField]
	private UnityEventTimeSpan onTimerChanged = new ();
	public event UnityAction<TimeSpan> OnTimerChanged { add => onTimerChanged.AddListener(value); remove => onTimerChanged.RemoveListener(value); }

	
	private float _timer;
	private int _score;
	private float _lastFlipTime;
	private bool _canFlipHoles;
	private Hole[] _holes;


	private IEnumerator Start()
	{
		yield return null;
		Initialize();
	}

	private void OnEnable()
	{
		this.OnAssemblyReload(Initialize);
#if UNITY_EDITOR
		grid.UnsubscribeHoles(OnBallScored, OnFail);
		grid.SubscribeHoles(OnBallScored, OnFail);
#endif
	}

	private void Initialize()
	{
		grid.UnsubscribeHoles(OnBallScored, OnFail);
		_holes = grid.GetHoles();

		_score = 0;
		_timer = baseGameTime;
		onScoreChanged.Invoke(_score);
		AudioController.Instance.PlayMusic(musicByIntensity[_currentIntensity]);
	}

	private void OnDisable()
	{
		grid.UnsubscribeHoles(OnBallScored, OnFail);
	}

	private void Update()
	{
		_timer -= Time.deltaTime;
		if (_timer <= 0f)
			EndGame();

		if (_lastFlipTime + holeFlipTime < Time.timeSinceLevelLoad && _canFlipHoles)
			FlipRandomHole();

		if (Time.timeSinceLevelLoad > _lastIntensityChange + 6f)
		{
			var newIntensity = _currentIntensity;

			if (_currentIntensity < musicIntensityTimerThreshold.Length - 1 &&
				_timer < musicIntensityTimerThreshold[_currentIntensity] + musicTransitionThreshold)
				newIntensity++;
			else if (_currentIntensity > 0 && _timer > musicIntensityTimerThreshold[_currentIntensity] - musicTransitionThreshold)
				newIntensity--;

			if (newIntensity != _currentIntensity)
			{
				_currentIntensity = newIntensity;
				AudioController.Instance.PlayMusic(musicByIntensity[_currentIntensity], duration: 3f, syncTime: true);
				_lastIntensityChange = Time.timeSinceLevelLoad;
				Debug.Log("Intensity: " + _currentIntensity);
			}
		}
		
		onTimerChanged.Invoke(TimeSpan.FromSeconds(_timer));
	}

	private void FlipRandomHole()
	{
		_lastFlipTime = Time.timeSinceLevelLoad;
		var hole = _holes[UnityEngine.Random.Range(0, _holes.Length)];
		if (_holes.Count(h=>(h.isGood && !h.isTweening) || (!h.isGood && h.isTweening)) <= 1 && hole.isGood)
			hole = _holes.FirstOrDefault(h => !h.isGood && !h.isTweening);
			
		hole?.Flip();
	}

	public void Throwing() => _canFlipHoles = false;

	public void ThrowFinished() => _canFlipHoles = true;
	
	private void OnBallScored(int id)
	{
		_score++;
		_timer += timerIncrement;
		onScoreChanged.Invoke(_score);
		grid.MoveHoleRandomly(id);
	}

	private void OnFail(int id)
	{
		_timer -= timerDecrement;
		onScoreChanged.Invoke(_score);
		grid.MoveHoleRandomly(id);
	}
	
	private void EndGame()
	{
		
	}
}