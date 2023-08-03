using Configs;
using GameCore.Events;
using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateLoadLevel : IGameStatePayloaded<SceneSets>
	{
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly FactoryService _factoryService;
		private readonly ProgressService _progressService;

		public GameStateLoadLevel(GameStateMachine stateMachine, LoadingScreen loadingScreen)
		{
			_stateMachine = stateMachine;
			_sceneLoader = new SceneLoader(loadingScreen);
			_factoryService = Services.FactoryService;
			_progressService = Services.ProgressService;
		}

		public async void Enter(SceneSets payload)
		{
			GlobalEventManager.OnLevelLoaded.AddListener(CreatePlayer);

			if (payload == SceneSets.MainMenu)
				await _sceneLoader.LoadMainMenu();
			else
			{
				_progressService.SaveLevel(payload);
				await _sceneLoader.LoadSceneSet(GetSceneContainer(payload));
			}

			_stateMachine.EnterGameLoopState();
		}

		public void Exit()
		{
		}

		private void CreatePlayer() =>
			TryPlacePlayer();

		private void TryPlacePlayer()
		{
			if (TryFindRespawn(out Transform respawn))
				_factoryService.CreatePlayer(respawn.position);
			else
				Debug.Log("Can't find player respawn");
		}

		private static bool TryFindRespawn(out Transform respawn) =>
			(respawn = GameObject.FindGameObjectWithTag(Tags.Respawn).transform) != null;

		private SceneContainer GetSceneContainer(SceneSets sceneSet) =>
			Services.ConfigService.ScenesConfig.GetSceneContainerWithSet(sceneSet);
	}
}