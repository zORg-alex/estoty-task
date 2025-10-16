using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Scripts.Extensions
{
	public static class DebugGizmos
	{
#if UNITY_EDITOR
		[InitializeOnEnterPlayMode]
#endif
		private static void Initialize()
		{
			_gizmos.Clear();
		}
		
		private static Dictionary<Object, List<Gizmo>> _gizmos = new Dictionary<Object, List<Gizmo>>();
		private class Gizmo
		{
			internal enum GizmoType
			{
				Lines,
				Points,
				Label,
			}

			public Vector3[] _positions;
			public Quaternion[] _rotations;
			public Color _color;
			public GizmoType _type;
			public string _label;
			public Vector3 _position;

			public void Draw()
			{
#if UNITY_EDITOR
				var c = Handles.color;
				Handles.color = _color;
				switch (_type)
				{
					case GizmoType.Lines:
						Handles.DrawAAPolyLine(_positions);
						break;
					case GizmoType.Points:
						for (int i = 0; i < _positions.Length; i++)
							HandlesExtensions.DrawAxis(_positions[i], _rotations[i]);
						break;
					case GizmoType.Label:
						Handles.Label(_position,  _label);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				Handles.color = c;
#endif
			}
		}

		public static void DrawLineGizmo(this MonoBehaviour monoBehaviour, params Vector3[] positions) =>
			DrawLineGizmo(monoBehaviour, Color.white, positions);
		public static void DrawLineGizmo(this MonoBehaviour monoBehaviour, Color color, params Vector3[] positions)
		{
			AddGizmo(monoBehaviour, new()
			{
				_color = color,
				_positions = positions,
				_type = Gizmo.GizmoType.Lines,
			});
		}

		public static void DrawPointsGizmo(this MonoBehaviour monoBehaviour, Vector3[] positions, Quaternion[] rotations) =>
			DrawPointsGizmo(monoBehaviour, Color.white, positions, rotations);
		public static void DrawPointsGizmo(this MonoBehaviour monoBehaviour, Color color, Vector3[] positions, Quaternion[] rotations)
		{
			Assert.AreEqual(positions.Length, rotations.Length);
			AddGizmo(monoBehaviour, new()
			{
				_color = color,
				_positions = positions,
				_rotations = rotations,
				_type = Gizmo.GizmoType.Points,
			});
		}

		public static void DrawLabelGizmo(this MonoBehaviour monoBehaviour, string label, Vector3 position) => DrawLabelGizmo(monoBehaviour, label, position, Color.white);
		public static void DrawLabelGizmo(this MonoBehaviour monoBehaviour, string label, Vector3 position, Color color)
		{
			AddGizmo(monoBehaviour, new()
			{
				_color = color,
				_position = position,
				_label = label,
				_type = Gizmo.GizmoType.Label,
			});
		}
		
		private static void AddGizmo(this MonoBehaviour monoBehaviour, Gizmo gizmo){
			if (_gizmos.TryGetValue(monoBehaviour, out var gizmos))
				gizmos.Add(gizmo);
			else
				_gizmos.Add(monoBehaviour, new List<Gizmo> { gizmo });
		}

		public static void DrawGizmos(this MonoBehaviour monoBehaviour)
		{
			if (_gizmos.TryGetValue(monoBehaviour, out var gizmos))
			{
				foreach (var gizmo in gizmos)
					gizmo.Draw();
			}
		}
		public static void ClearGizmos(this MonoBehaviour monoBehaviour)
		{
			if (_gizmos.TryGetValue(monoBehaviour, out var gizmos))
			{
				gizmos.Clear();
			}
		}

		public static string GetJson()
		{
			return string.Join("\n\n", _gizmos.Select(gg=>gg.Key.name + $":\n\t" + string.Join("\n\t", gg.Value.Select(g=>$"{g._type} {string.Join(", ", g?._positions?? Array.Empty<Vector3>())}; {string.Join(", ", g?._rotations?? Array.Empty<Quaternion>())} {g._color} {g._label}  {g._position}"))));
		}
	}
}