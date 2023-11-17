using UnityEngine.SceneManagement;

namespace Utils.Constants
{
	public static class Scenes
	{
		private const string SCENE_PATH = "Assets/Scenes/";
		private const string SCENE_FORMAT = ".unity";

		public const string BOOTSTRAP = "Bootstrap";
		public const string LOADING_SCREEN = "LoadingScreen";
		public const string MAIN_MENU_UI = "MainMenuUI";
		public const string LEVEL1 = "Level1";
		public const string LEVEL2 = "Level2";
		public const string LEVEL_UI = "LevelUI";

		public static string ActiveScene => SceneManager.GetActiveScene().name;

		public static string GetScenePath(string sceneName) =>
			SCENE_PATH + sceneName + SCENE_FORMAT;

#if UNITY_EDITOR
		public static void OpenScene(string sceneName) =>
			UnityEditor.SceneManagement.EditorSceneManager.OpenScene(GetScenePath(sceneName));
#endif
	}

	public enum SceneSets
	{
		MainMenu = 0,
		Level1 = 1,
		Level2 = 2
	}
}