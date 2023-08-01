using UnityEditor;
using UnityEngine;
using Utils.Constants;

namespace Utils.Debug
{
	public static class SceneViewLabels
	{
		public static void DrawHandlesLabel(Vector3 position, string text, UnityEngine.Color color)
		{
			var style = new GUIStyle();
			style.normal.textColor = Colors.Red;
			Handles.Label(position, text, style);
		}

		public static void DrawGizmosWireSphere(Vector3 center, float radius, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawWireSphere(center, radius);
		}

		public static void DrawGizmosWireCube(Vector3 center, Vector3 size, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawWireCube(center, size);
		}
	}
}