using UnityEngine.SceneManagement;

namespace Utils.Constants
{
	public static class Scenes
	{
		private const string SCENE_PATH = "Assets/Scenes/";
		private const string SCENE_FORMAT = ".unity";
		public const string BOOTSTRAP_SCENE = "bootstrap";

		public static string ActiveScene => SceneManager.GetActiveScene().name;

		public static string GetScenePath(string sceneName) =>
			SCENE_PATH + sceneName + SCENE_FORMAT;
	}

	public enum SceneSets
	{
		MainMenu = 0,
		Level1 = 1,
		Level2 = 2,
		Level3 = 3,
		Level4 = 4,
		Level5 = 5,
	}
}