#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Scripts.Extensions
{
	public abstract class LocalEditorTool<T> : EditorTool, IDrawSelectedHandles where T : MonoBehaviour
	{
		private bool _isEdited;
		private Transform _transform;
		protected T _script;
		
		public override GUIContent toolbarIcon => GetIcon();
		private void OnEnable()
		{
			_script = target as T;
			if (_script == null) return;
			_transform = _script.transform;
		}
		
		public override void OnActivated()
		{
			_script = target as T;
			if (_script == null) return;
			_transform = _script.transform;
			_isEdited = true;
		}

		public override void OnWillBeDeactivated()
		{
			_isEdited = false;
		}
		
		public void OnDrawHandles()
		{
			if (!_transform || !_script) return;
			Handles.matrix = _transform.localToWorldMatrix;

			DrawHandles();
		}
		public override void OnToolGUI(EditorWindow window)
		{
			if (!(window is SceneView sceneView))
				return;
			if (_script == null) return;

			Handles.matrix = _transform.localToWorldMatrix;

			if (_isEdited)
			{
				WhileEdited();
			}
		}

		protected abstract void WhileEdited();

		protected abstract void DrawHandles();
		
		protected abstract GUIContent GetIcon();
	}
}
#endif