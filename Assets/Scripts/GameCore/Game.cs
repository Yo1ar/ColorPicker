using GameCore.GameServices;
using GameCore.GameUI;
using GameCore.StateMachine;
using UnityEditor;
using UnityEngine;

namespace GameCore
{
	public sealed class Game
	{
		private GameStateMachine _gameStateMachine;
		private Services _services;
		private readonly LoadingScreen _loadingScreen;

		public Game(LoadingScreen loadingScreen)
		{
			_loadingScreen = loadingScreen;
			StartGameFlow();
		}

		public static void Quit()
		{
#if UNITY_EDITOR || UNITY_EDITOR_64
			EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
		}

		public static void Pause() =>
			Time.timeScale = 0;

		public static void Unpause() =>
			Time.timeScale = 1;

		private async void StartGameFlow()
		{
			_services = new Services();
			await _services.InitServices();

			_gameStateMachine = new GameStateMachine(_loadingScreen);
			_gameStateMachine.EnterBootstrapState();
		}
	}
}