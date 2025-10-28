using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Extensions
{
	public static class MonobehaviourExtensions
	{
		public static bool TrySetEnabled(this MonoBehaviour self, bool enabled)
		{
			if (!self) return false;
			self.enabled = enabled;
			return true;
		}

		public static bool TrySetEnabled(this Collider self, bool enabled)
		{
			if (!self) return false;
			self.enabled = enabled;
			return true;
		}

		public static void DestroyEditorSafe(this Object instance)
		{
			if (Application.isPlaying)
				Object.Destroy(instance);
			else
				Object.DestroyImmediate(instance);
		}

		/// <summary>
		/// Use <_code>
		/// private void Start() => InitializeContext();
		/// private void OnEnable() => this.OnAssemblyReload(InitializeContext);</_code>
		/// </summary>
		/// <param name="mb"></param>
		/// <param name="coroutine"></param>
		public static void OnAssemblyReload(this MonoBehaviour mb, Func<IEnumerator> coroutine)
		{
#if UNITY_EDITOR
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload += Handler;
			void Handler() => mb.StartCoroutine(coroutine());
#endif
		}
	}
}