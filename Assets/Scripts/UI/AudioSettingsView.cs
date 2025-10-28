using Scripts.Audio;
using Scripts.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class AudioSettingsView : UIContext, IInputBack, IInputEsc//TabView
	{
		[SerializeField] private Slider masterVolume;
		[SerializeField] private Slider musicVolume;
		[SerializeField] private Slider uiVolume;
		[SerializeField] private Slider sfxVolume;

		protected override void OnInitialize()
		{
			if (masterVolume)
			{
				masterVolume.value = AudioController.Instance.GetMasterVolume();
				masterVolume.onValueChanged.AddListener(AudioController.Instance.SetMasterVolume);
			}
			if (musicVolume)
			{
				musicVolume.value = AudioController.Instance.GetMusicVolume();
				musicVolume.onValueChanged.AddListener(AudioController.Instance.SetMusicVolume);
			}
			if (uiVolume)
			{
				uiVolume.value = AudioController.Instance.GetUIVolume();
				uiVolume.onValueChanged.AddListener(AudioController.Instance.SetUIVolume);
			}
			if (sfxVolume)
			{
				sfxVolume.value = AudioController.Instance.GetSFXVolume();
				sfxVolume.onValueChanged.AddListener(AudioController.Instance.SetSFXVolume);
			}
		}

		public override void OnHide()
		{

		}

		public override void OnShow()
		{

		}

		public void OnBack() => Hide();
		public void OnEsc() => Hide();
	}
}