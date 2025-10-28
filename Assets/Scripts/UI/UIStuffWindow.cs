#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIStuffWindow : EditorWindow
{
	private static Scene _scene;
	private static UIStuffWindow _window;

	//[InitializeOnLoadMethod]
	private static void InitializeOnLoad()
	{
		EditorApplication.update += Update;
		void Update()
		{
			ShowWindow();
			EditorApplication.update -= Update;
		}
	}

	[MenuItem("Tools/UI Stuff")]
	public static void ShowWindow()
	{
		if (_window != null) return;
		_window = GetWindow<UIStuffWindow>("UIStuffWindow");
		_window.titleContent = new GUIContent("UI Stuff");
	}
	private void OnEnable()
	{
		maxSize = new Vector2(400f, 22f);
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add UI Scene"))
		{
			OpenUIScene();
		}
		if (GUILayout.Button("Remove UI Scene"))
		{
			CloseUIScene();
		}
		EditorGUILayout.EndHorizontal();
	}

	private static void CloseUIScene()
	{
		if (_scene == null || _scene.name == null)
			_scene = EditorSceneManager.GetSceneByPath(EditorBuildSettings.scenes[0].path);
		EditorSceneManager.CloseScene(_scene, true);
	}

	private static void OpenUIScene()
	{
		var scene = EditorBuildSettings.scenes[0];
		_scene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
	}

	private void OnDestroy()
	{

	}

	private class Settings
	{

	}
}
#endif