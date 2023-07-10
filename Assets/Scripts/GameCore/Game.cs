using System.Threading.Tasks;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Constants;

namespace GameCore
{
	public sealed class Game
	{
		private GameStateMachine _gameStateMachine;
		private ServiceLocator _serviceLocator;

		public Game() =>
			StartGameFlow();

		public static void Quit() =>
#if UNITY_EDITOR || UNITY_EDITOR_64
			EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif

		public static void Pause() =>
			Time.timeScale = 0;

		public static void Unpause() =>
			Time.timeScale = 1;

		private async void StartGameFlow()
		{
			await LoadGameUI();
			_serviceLocator = new ServiceLocator();
			StartGameStateMachine();
		}

		private async Task LoadGameUI()
		{
			AsyncOperation loading = SceneManager.LoadSceneAsync(Scenes.GameUI, LoadSceneMode.Additive);

			while (!loading.isDone)
				await Task.Yield();
		}

		private void StartGameStateMachine()
		{
			var stateMachineObject = new GameObject("GameStateMachine");
			_gameStateMachine = stateMachineObject.AddComponent<GameStateMachine>();
			_gameStateMachine.Init(_serviceLocator);
			_gameStateMachine.EnterBootstrapState();
		}
	}
}