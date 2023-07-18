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
			Scenes.OpenScene(Scenes.Bootstrap);

		[MenuItem("Scenes/GameUI")]
		public static void OpenScene_GameUI() =>
			Scenes.OpenScene(Scenes.GameUI);

		[MenuItem("Scenes/MainMenuUI")]
		public static void OpenScene_MainMenuUI() =>
			Scenes.OpenScene(Scenes.MainMenuUI);

		[MenuItem("Scenes/Level1")]
		public static void OpenScene_Level1() =>
			Scenes.OpenScene(Scenes.Level1);

		[MenuItem("Scenes/Level2")]
		public static void OpenScene_Level2() =>
			Scenes.OpenScene(Scenes.Level2);

		[MenuItem("Scenes/LevelUI")]
		public static void OpenScene_LevelUI() =>
			Scenes.OpenScene(Scenes.LevelUI);

		#endregion
	}
}