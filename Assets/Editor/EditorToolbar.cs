using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;
using Utils.Constants;

namespace Editor
{
	[InitializeOnLoad]
	public static class EditorToolbar
	{
		private static bool BootstrapEnabled;

		private static bool bootstrapEnabled
		{
			get => BootstrapEnabled;
			set
			{
				BootstrapEnabled = value;
				EditorSceneManager.playModeStartScene = LoadBootstrapSceneAsset(
					BootstrapEnabled
						? Scenes.Bootstrap
						: Scenes.activeScene);
			}
		}

		static EditorToolbar()
		{
			ToolbarExtender.LeftToolbarGUI.Add(DrawLeftGUI);
			bootstrapEnabled = true;
		}

		private static void DrawLeftGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Focus on Player"))
				FocusOnPlayer();

			GUI.changed = false;

			GUILayout.Toggle(BootstrapEnabled, "Bootstrap");
			if (GUI.changed)
				bootstrapEnabled = !bootstrapEnabled;
		}

		private static SceneAsset LoadBootstrapSceneAsset(string sceneName) =>
			AssetDatabase.LoadAssetAtPath<SceneAsset>(Scenes.GetScenePath(sceneName));

		private static void FocusOnPlayer()
		{
			GameObject player = GameObject.FindWithTag("Player");

			if (player == null)
				return;

			Selection.activeGameObject = player;
			EditorGUIUtility.PingObject(player);
		}
	}
}