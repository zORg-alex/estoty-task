using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Scripts.Extensions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Audio
{
	public class AudioPool : MonoBehaviour
	{
		private static AudioPool _instance;
		public static AudioPool Instance => Singletons.GetOrFindByType(ref _instance);

		private ObjectPool<AudioSource> pool;

		[SerializeField] private void Start() => Initialize();
		private void OnEnable()
		{
			if (this.OnEnableDestroyWholeIfCopy(ref _instance))
				this.OnAssemblyReload(Initialize);
		}

		private void Initialize()
		{
			pool = new ObjectPool<AudioSource>(CreateSource, OnGetSource, OnReleaseSource, OnDestroySource);
		}

		private AudioSource CreateSource()
		{
			var r = new GameObject("AudioSource " + pool.CountInactive).AddComponent<AudioSource>();
			return r;
		}

		private void OnGetSource(AudioSource source)
		{
			source.gameObject.SetActive(true);
		}

		private void OnReleaseSource(AudioSource source)
		{
			source.gameObject.SetActive(false);
		}

		private void OnDestroySource(AudioSource source)
		{
			Destroy(source.gameObject);
		}

		public static void PlayAt(AudioClip clip, Vector3 position, out AudioSource source)
		{
			Instance.pool.Get(out source);
			source.clip = clip;
			source.loop = false;
			source.Play();

			var captSource = source;
			UniTask.WaitForSeconds(clip.length + .1f).ContinueWith(() => Instance.pool.Release(captSource)).Forget();
		}

		public static AudioSource GetPooledSource(out Action release)
		{
			Instance.pool.Get(out var source);
			release = () => Instance.pool.Release(source);
			return source;
		}
	}
}