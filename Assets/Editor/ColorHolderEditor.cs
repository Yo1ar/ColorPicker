using System.Collections.Generic;
using Components.Color;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(ColorHolder))]
	[CanEditMultipleObjects]
	public class ColorHolderEditor : UnityEditor.Editor
	{
		private SerializedProperty _color;
		private List<SpriteRenderer> _renderers;
		private string[] _allColors;

		private void OnEnable()
		{
			SetSpriteRendererColor();
			serializedObject.ApplyModifiedProperties();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!GUI.changed)
				return;

			SetSpriteRendererColor();
		}

		private void SetSpriteRendererColor()
		{
			foreach (Object obj in targets)
				((ColorHolder)obj).Recolor();
		}
	}
}