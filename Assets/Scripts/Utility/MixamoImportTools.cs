﻿using UnityEditor;
using UnityEngine;

public class MixamoImportTools : EditorWindow {
	[MenuItem("Window/Mixamo Import Tools")]
	private static void OpenEditorWindow() {
		GetWindow<MixamoImportTools>().Show();
	}
	void FixNamingInAnimations() {
		foreach (GameObject o in Selection.objects) {
			ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(o));
			var clips = modelImporter.defaultClipAnimations;
			foreach (var c in clips) {
				if (c.name == "mixamo.com") c.name = o.name;
			}
			modelImporter.clipAnimations = clips;
		}
	}
	private void OnGUI()
	{
		if (GUILayout.Button("Fix naming in Animations"))
		{
			FixNamingInAnimations();
		}
	}
}
