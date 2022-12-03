using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public Controls Controls;
	public static InputController Instance;
	public PlayerMovement playerMovement;
	private IEnumerator _playerIinputCR;

	private void OnEnable()
	{
		Controls = new Controls();
		Instance = this;
		if (playerMovement != null)
			StartPlayerInputCR();
	}

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
		while (true)
		{
			yield return null;
			playerMovement.Move(Controls.Player.Move.ReadValue<Vector2>());
		}
	}

	public void EnablePlayerInput() => Controls.Player.Enable();
	public void DisablePlayerInout() => Controls.Player.Disable();
}
