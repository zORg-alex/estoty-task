using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
	public Controls Controls;
	public static InputController Instance;
	private IEnumerator _playerIinputCR;
	public Vector2Event OnPlayerMove = new Vector2Event();
	public FloatEvent OnPlayerTurn = new FloatEvent();
	public FloatEvent OnPlayerZoom = new FloatEvent();
	public UnityEvent OnInteractPressed = new UnityEvent();
	public UnityEvent OnInteractReleased = new UnityEvent();

	private void OnEnable()
	{
		Controls = new Controls();
		Instance = this;
		
		StartPlayerInputCR();
		Controls.Player.Interact.started += Interact_pressed;
		Controls.Player.Interact.canceled += Interact_released;
	}
	private void OnDisable()
	{
		Controls.Player.Interact.started -= Interact_pressed;
		Controls.Player.Interact.canceled -= Interact_released;
	}

	private void Interact_released(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnInteractReleased.Invoke();
	private void Interact_pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnInteractPressed.Invoke();

	private void StartPlayerInputCR()
	{
		Controls.Player.Enable();
		if (_playerIinputCR != null) StopCoroutine(_playerIinputCR);
		_playerIinputCR = PlayerInputCR();
		StartCoroutine(_playerIinputCR);
	}
	private void StopPlayerInput()
	{
		Controls.Player.Disable();
		if (_playerIinputCR != null)
			StopCoroutine(_playerIinputCR);
		_playerIinputCR = null;
	}

	IEnumerator PlayerInputCR()
	{
		while (Controls.Player.enabled)
		{
			yield return null;
			OnPlayerMove.Invoke(Controls.Player.Move.ReadValue<Vector2>());
			OnPlayerTurn.Invoke(Controls.Player.Turn.ReadValue<float>());
			OnPlayerZoom.Invoke(Controls.Player.Zoom.ReadValue<float>());
		}
		_playerIinputCR = null;
	}
}

[Serializable]
public class Vector2Event : UnityEvent<Vector2> { }
[Serializable]
public class FloatEvent : UnityEvent<float> { }