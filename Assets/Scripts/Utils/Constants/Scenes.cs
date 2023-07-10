using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Utils.Constants
{
	public static class Scenes
	{
		private const string ScenePath = "Assets/Scenes/";
		private const string SceneFormat = ".unity";
		
		public const string Bootstrap = "Bootstrap";
		public const string GameUI = "GameUI";
		public const string MainMenu = "MainMenu";
		public const string MainMenuUI = "MainMenuUI";
		public const string Level1 = "Level1";
		public const string Level2 = "Level2";
		public const string LevelUI = "LevelUI";

		public static string activeScene => SceneManager.GetActiveScene().name;
		
		public static string GetScenePath(string sceneName) => 
			ScenePath + sceneName + SceneFormat;
		
		public static void OpenScene(string sceneName) =>
			EditorSceneManager.OpenScene(GetScenePath(sceneName));

	}
	
	public enum SceneSets
	{
		MainMenu = 0,
		Level1 = 1,
		Level2 = 2
	}
}