//using Sirenix.OdinInspector;

using System;
using PrimeTween;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.Audio;
namespace Scripts.Audio
{
    public class AudioController : MonoBehaviour
    {
    	private static AudioController _instance;
    	public static AudioController Instance => Singletons.GetOrFindByType(ref _instance);
    	[SerializeField][Required]
    	private AudioSource _uiAudioSource;
    	[SerializeField][Required]
    	private AudioSource[] _musicAudioSource;
    	private int _lastMusicSourceId;
    	[SerializeField, Required]
    	private AudioMixer _masterMixer;
    	[Required, SerializeField]
    	private string _masterVolumePath = "Master";
    	[Required, SerializeField]
    	private string _musicVolumePath = "Music";
    	[Required, SerializeField]
    	private string _sfxVolumePath = "SFX";
    	[Required, SerializeField]
    	private string _uiVolumePath = "UI";

    	private void OnEnable()
    	{
    		if (this.OnEnableDestroyIfCopy(ref _instance)) return;
    	}

    	public  void PlayUIClip(AudioClip clip)
    	{
    		_uiAudioSource.clip = clip;
    		_uiAudioSource.Play();
    	}
    	public void PlayMusic(AudioClip music, bool loop = true, float duration = 3f)
    	{
            if (_musicAudioSource.Length == 0) { Debug.LogError("MusicSources Missing"); return; }
            int lastInd = _lastMusicSourceId++ % _musicAudioSource.Length;
            int ind = _lastMusicSourceId % _musicAudioSource.Length;
            if (_musicAudioSource.Length > 1 && _musicAudioSource[lastInd].volume > 0f)
                Tween.AudioVolume(_musicAudioSource[lastInd], 0f, duration);
            _musicAudioSource[ind].loop = loop;
            _musicAudioSource[ind].clip = music;
            _musicAudioSource[ind].Play();
			if (_musicAudioSource[ind].volume < 1f)
				Tween.AudioVolume(_musicAudioSource[ind], 1f, duration);
        }

    	internal void StopMusic(float duration = 3f)
    	{
    		int ind = _lastMusicSourceId % _musicAudioSource.Length;
    		Tween.AudioVolume(_musicAudioSource[ind], 0, duration);
    	}

    	public void SetMasterVolume(float value) => SetVolume(_masterVolumePath, value);
    	public void SetMusicVolume(float value) => SetVolume(_musicVolumePath, value);
    	public void SetSFXVolume(float value) => SetVolume(_sfxVolumePath, value);
    	public void SetUIVolume(float value) => SetVolume(_uiVolumePath, value);
    	private void SetVolume(string path, float value)
    	{
    		PlayerPrefs.SetFloat($"Settings.{path}Volume", value);
    		_masterMixer.SetFloat(path, Mathf.Log(value) * 20);
    	}
    	public float GetMasterVolume() => GetVolume(_masterVolumePath);
    	public float GetMusicVolume() => GetVolume(_musicVolumePath);
    	public float GetSFXVolume() => GetVolume(_sfxVolumePath);
    	public float GetUIVolume() => GetVolume(_uiVolumePath);
    	private float GetVolume(string path) => PlayerPrefs.GetFloat($"Settings.{path}Volume", 1f);

    }
}
