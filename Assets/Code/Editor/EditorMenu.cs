using UnityEditor;
using UnityEngine;
using Utils.Constants;

namespace Editor
{
	public static class EditorMenu
	{
		[MenuItem("Tools/Player Prefs/Clear All Prefs")]
		public static void ClearPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}

		[MenuItem("Tools/Player Prefs/Clear PlayerProgress")]
		public static void ClearPlayerProgress()
		{
			PlayerPrefs.DeleteKey("PlayerProgress");
			PlayerPrefs.Save();
		}

#region Scenes
		[MenuItem("Scenes/Bootstrap")]
		public static void OpenScene_Bootstrap() =>
			Scenes.OpenScene(Scenes.BOOTSTRAP);

		[MenuItem("Scenes/MainMenuUI")]
		public static void OpenScene_MainMenuUI() =>
			Scenes.OpenScene(Scenes.MAIN_MENU_UI);

		[MenuItem("Scenes/Level1")]
		public static void OpenScene_Level1() =>
			Scenes.OpenScene(Scenes.LEVEL1);

		[MenuItem("Scenes/Level2")]
		public static void OpenScene_Level2() =>
			Scenes.OpenScene(Scenes.LEVEL2);

		[MenuItem("Scenes/LevelUI")]
		public static void OpenScene_LevelUI() =>
			Scenes.OpenScene(Scenes.LEVEL_UI);
#endregion // Scenes
	}
}