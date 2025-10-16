#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Scripts.Extensions
{
	public static class HandlesExtensions
	{
		private static Vector3[] _rectangleHandlePointsCache = new Vector3[5];
#if UNITY_EDITOR
		public static void RectangleHandleCap(int controlID, Vector3 position, Quaternion rotation, float size,
											  EventType eventType)
		{
			switch (eventType)
			{
				case EventType.MouseMove:
				case EventType.Layout:
					HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangle(position, rotation, size));
					break;
				case EventType.Repaint:
				{
					Vector3 vector = rotation * new Vector3(size, 0f, 0f);
					Vector3 vector2 = rotation * new Vector3(0f, size, 0f);
					_rectangleHandlePointsCache[0] = position + vector + vector2;
					_rectangleHandlePointsCache[1] = position + vector - vector2;
					_rectangleHandlePointsCache[2] = position - vector - vector2;
					_rectangleHandlePointsCache[3] = position - vector + vector2;
					_rectangleHandlePointsCache[4] = position + vector + vector2;
					var color = Handles.color;
					Handles.color = color.MultiplyAlpha(.3f);
					var ztest = Handles.zTest;
					Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
					Handles.DrawAAConvexPolygon(_rectangleHandlePointsCache);
					Handles.color = color;
					Handles.DrawPolyLine(_rectangleHandlePointsCache);
					Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
					Handles.color = color.MultiplyAlpha(.3f);
					Handles.DrawPolyLine(_rectangleHandlePointsCache);
					Handles.zTest = ztest;
					Handles.color = color;
					break;
				}
			}
		}

		public static void DrawAxis(Transform transform, float alpha = 1f) =>
			DrawAxis(transform.position, transform.rotation, alpha);

		public static void DrawAxis(Vector3 position, Quaternion rotation, float alpha = 1f)
		{
			var color = Handles.color;
			Handles.color = Color.red.SetAlpha(alpha);
			Handles.DrawLine(position, position + rotation * Vector3.right);
			Handles.color = Color.green.SetAlpha(alpha);
			Handles.DrawLine(position, position + rotation * Vector3.up);
			Handles.color = Color.blue.SetAlpha(alpha);
			Handles.DrawLine(position, position + rotation * Vector3.forward);
			Handles.color = color;
		}
#else

		public static void RectangleHandleCap(int controlID, Vector3 position, Quaternion rotation, float size,
											  EventType eventType) {}
		public static void DrawAxis(Transform transform, float alpha = 1f) {}
		public static void DrawAxis(Vector3 position, Quaternion rotation, float alpha = 1f) {}
#endif
	}
}