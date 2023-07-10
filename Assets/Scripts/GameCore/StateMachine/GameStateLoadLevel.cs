using Configs;
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

		public GameStateLoadLevel(GameStateMachine stateMachine, LoadingScreen loadingScreen)
		{
			_stateMachine = stateMachine;
			_sceneLoader = new SceneLoader(loadingScreen);
			_factoryService = ServiceLocator.factoryService;
		}

		public async void Enter(SceneSets payload)
		{
			await _sceneLoader.LoadSceneSet(GetSceneSet(payload), OnLevelLoaded);
			_stateMachine.EnterGameLoopState();
		}

		private void OnLevelLoaded()
		{
			TryPlacePlayer();
		}

		private void TryPlacePlayer()
		{
			if (TryFindRespawn(out Transform respawn))
				_factoryService.CreatePlayer(respawn.position);
		}

		private static bool TryFindRespawn(out Transform respawn) =>
			(respawn = GameObject.FindGameObjectWithTag(Tags.Respawn).transform) != null;

		public void Update()
		{
		}

		public void Exit()
		{
		}

		private SceneSet GetSceneSet(SceneSets sceneSet) =>
			ServiceLocator.configService.scenesConfig.GetSceneSet(sceneSet);
	}
}