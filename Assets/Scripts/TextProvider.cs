using System;
using Scripts.Extensions;
using TMPro;
using UnityEngine;

public class TextProvider : MonoBehaviour
{
	private TMP_Text _text;

	[SerializeField] private string textTemplate = "{0}";
	
	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
		if (!TryGetComponent(out _text))
			Debug.LogError("No text component found", this);
	}
	
	public void SetText(string text) => 
		_text?.SetText(string.Format(textTemplate, text));
	public void SetText(int number) => 
		_text?.SetText(string.Format(textTemplate, number));
	public void SetText(float number) => 
		_text?.SetText(string.Format(textTemplate, number));
	public void SetText(TimeSpan timeSpan) => 
		_text?.SetText(string.Format(textTemplate, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
}