using System;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class TrajectoryTracer : MonoBehaviour
{
	private bool _hide;

	[SerializeField, Required] private Transform trajectoryVisual;
	private GameObject _trajectoryVisualGameObject;

	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
		if (!trajectoryVisual) { Debug.LogError("trajectoryVisual is null"); return; }
		_trajectoryVisualGameObject = trajectoryVisual.gameObject;
		_trajectoryVisualGameObject.SetActive(false);
	}
	public void OnTrajectoryChanged(Vector2 value)
	{
		if (_hide)
		{
			trajectoryVisual.gameObject.SetActive(true);
			_hide = false;
		}

		if (value.sqrMagnitude < .001f)
		{
			Hide();
			return;
		}
		
		var direction = new Vector3(value.x, 0, value.y);
		var rotation = Quaternion.LookRotation(direction, Vector3.up);
		trajectoryVisual.localRotation = rotation;
	}

	public void Hide()
	{
		_hide = true;
		trajectoryVisual.gameObject.SetActive(false);
	}

	private void Update()
	{
		
	}
}