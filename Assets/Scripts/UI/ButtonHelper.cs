using System;
using Scripts.Extensions;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI
{
	[ExecuteAlways]
	public class ButtonHelper : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private Sprite sprite;
		[SerializeField] private Button button;
		[SerializeField] private Toggle toggle;
		[SerializeField, OnValueChanged(nameof(TextChanged))] private string text;
		[SerializeField] private TMP_Text tmpText;
		private void TextChanged() => tmpText?.SetText(text);

		public void SetIcon(Sprite sprite)
		{
			if (icon.TrySetEnabled(sprite != null))
				icon.SetSprite(sprite);
		}

		public void SetTitle(string title)
		{
			
		}

		public void SetDescription(string description)
		{
			
		}

		public void SetOnClick(UnityAction onClick)
		{
			button?.onClick.AddListener(onClick);
		}

		private void OnEnable()
		{
			if (icon)
				icon.SetSprite(sprite);
			if (tmpText)
				tmpText.SetText(text);
		}
	}
}