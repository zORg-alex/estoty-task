using System;
using System.Collections;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.MainSystems
{
	/// <summary>
	/// Will make sure that Main scene (0 in build order) will be loaded on any scene load,
	/// all calls to singletons in that scene should happen on Start, when it is loaded
	/// </summary>
	public class MainSceneController : MonoBehaviour
	{
		private static Scene _menuScene;
		private static AsyncOperation _loadScene;
		private static bool _primaryLoad;
		public static bool IsPrimarilyLoaded => _primaryLoad;

#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static void InitializeEditorOnLoad()
		{
			EditorSceneManager.activeSceneChangedInEditMode += EditorSceneManager_activeSceneChangedInEditMode;
			void EditorSceneManager_activeSceneChangedInEditMode(Scene a, Scene b)
			{
				if (b.buildIndex == 0) return;

				_menuScene = default;
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					var scene = SceneManager.GetSceneAt(i);
					if (scene.buildIndex == 0) _menuScene = scene;
				}
				if (!_menuScene.isLoaded)
				{
					var scene = EditorBuildSettings.scenes[0];
					EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
				}
			}
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void AfterSceneLoadInitizlize()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (scene.buildIndex == 0) _menuScene = scene;
			}
			if (string.IsNullOrEmpty(_menuScene.name))
				_loadScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
			else
				_primaryLoad = true;
		}
	}
}