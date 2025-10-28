using Scripts.Extensions;
using Scripts.MainSystems;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.MainSystems
{
	public class MenuUIController : MonoBehaviour
	{
		private void Start() => Initialize();
		private void OnEnable() => this.OnAssemblyReload(Initialize);

		private void Initialize()
		{
			if (Application.isPlaying && !SceneManager.GetSceneByName("MenuUI").isLoaded)
				SceneManager.LoadSceneAsync("MenuUI", LoadSceneMode.Additive);

#if UNITY_EDITOR
			EditorSceneManager.sceneLoaded += SheckIfMainMenuLoaded;
#endif
		}
#if UNITY_EDITOR
		private void SheckIfMainMenuLoaded(Scene arg0, LoadSceneMode arg1)
		{
			//if (!Application.isPlaying && GameController.Instance.IsInMainMenu)
			//	EditorSceneManager.OpenScene("Assets/Scenes/MenuUI.unity", OpenSceneMode.Additive);
		}
#endif
	}
}