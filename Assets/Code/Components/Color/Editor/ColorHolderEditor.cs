using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorHolderBase), true)]
[CanEditMultipleObjects]
public class ColorHolderEditor : UnityEditor.Editor
{
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
			((ColorHolderBase)obj).Recolor();
	}
}