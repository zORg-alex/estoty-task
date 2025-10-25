using System.Collections;
using Scripts.Extensions;
using TriInspector;
using UnityEditor.EditorTools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ThrowMechanic : MonoBehaviour
{
	[SerializeField, Required] private Rigidbody ballPrefab;
	[SerializeField] private Vector3 ballInitialPosition;
	private Vector2 _throwValue;
	private Rigidbody ball;
	[SerializeField] private float fullThrowForce = 3f;
	[SerializeField] private float fullThrowVerticalForce = 2f;

	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
		if (!ball)
			SpawnNewBall();
	}
	
	public void OnThrow(Vector2 value)
	{
		if (!ball) return;
		
		_throwValue = value;
		StartCoroutine(Throw());
	}

	public void OnValueChanged(Vector2 value)
	{
		_throwValue = value;
	}

	private IEnumerator Throw()
	{
		ball.isKinematic = false;
		ball.AddForce(-_throwValue.x * fullThrowForce, _throwValue.magnitude * fullThrowVerticalForce, -_throwValue.y * fullThrowForce);
		ball = null;
		yield return new WaitForSeconds(1f);

		SpawnNewBall();
	}

	private void SpawnNewBall()
	{
		ball = Instantiate(ballPrefab);
		ball.transform.position = ballInitialPosition;
		ball.isKinematic = true;
	}


#if UNITY_EDITOR
	
	[EditorTool("Origin", typeof(ThrowMechanic))]
	class InnerBoundsTool : LocalEditorTool<ThrowMechanic>
	{
		protected override GUIContent GetIcon() => EditorGUIUtility.IconContent("Transform Icon", "Edit Origin");

		protected override void WhileEdited()
		{
			
			var pos = Handles.DoPositionHandle(_script.ballInitialPosition,
				Quaternion.identity);

			if (_script.ballInitialPosition != pos)
			{
				Undo.RecordObject(target, "Move ballInitialPosition");
				_script.ballInitialPosition = pos;
			}
		}

		protected override void DrawHandles()
		{
			Handles.DrawWireDisc(_script.ballInitialPosition, Vector3.up, HandleUtility.GetHandleSize(_script.ballInitialPosition) * .2f);
		}
	}

	[CustomEditor(typeof(ThrowMechanic))]
	public class ThrowMechanicInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Origin"), this);
			GUILayout.Space(5);
			this.DrawDefaultInspector();
		}
	}
#endif
}