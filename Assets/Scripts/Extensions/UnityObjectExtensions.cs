using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Extensions
{
	public static class UnityObjectExtensions
	{
#if UNITY_EDITOR
#else
		public delegate void ReloadCallback();
#endif
		/// <summary>
		/// Only object null check
		/// </summary>
		/// <param name="unityObject"></param>
		/// <returns></returns>
		public static bool IsNullOnly(this Object @object) => (object)@object == null;

		/// <summary>
		/// Returns self if alive else null. Helps get rid of exceptions when object was destroyed.
		/// </summary>
		public static T OrNull<T>(this T @object) where T : UnityEngine.Object => @object ? @object : null;

		/// <summary>
		/// Use <_code>
		/// private void Start() => InitializeContext();
		/// private void OnEnable() => this.OnAssemblyReload(InitializeContext);</_code>
		/// </summary>
#if UNITY_EDITOR
		public static void OnAssemblyReload(this Object @object,
											AssemblyReloadEvents.AssemblyReloadCallback after,
											AssemblyReloadEvents.AssemblyReloadCallback before = null)
		{
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= after;
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload += after;
			if (before != null)
			{
				UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= before;
				UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += before;
			}
		}
#else
		public static void OnAssemblyReload(this Object @object,
		ReloadCallback after, ReloadCallback before = null) {}
#endif

		[Flags]
		public enum ReloadFlags
		{
			None = 0,
			SkipInitIfNotPlaying = 1 << 0,
			SkipDeinitIfNotPlaying = 1 << 1,
			SkipAllIfNotPlaying = SkipInitIfNotPlaying | SkipDeinitIfNotPlaying,
		}

		/// <summary>
		/// Use <_code>
		/// private void Start() => InitializeContext();
		/// private void OnEnable() => this.OnAssemblyReload(Initialize, Deinitialize, ReloadFlags.None);</_code>
		/// In case it's not supposed to init or deinit before reload, you can restrict with flags
		/// </summary>
#if UNITY_EDITOR
		public static void OnAssemblyReload(this Object @object,
											AssemblyReloadEvents.AssemblyReloadCallback after,
											AssemblyReloadEvents.AssemblyReloadCallback before, ReloadFlags flags)
		{
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= after;
			if (Application.isPlaying || !flags.HasFlag(ReloadFlags.SkipInitIfNotPlaying))
				UnityEditor.AssemblyReloadEvents.afterAssemblyReload += after;

			UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= before;
			if (before == null || (!Application.isPlaying && flags.HasFlag(ReloadFlags.SkipDeinitIfNotPlaying))) return;
			UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += before;
		}
#else
		public static void OnAssemblyReload(this Object @object, ReloadCallback after, ReloadCallback before, ReloadFlags flags) { }
#endif
	}
}