using System.Threading.Tasks;
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

		public void Enter(SceneSets payload)
		{
			GlobalEventManager.OnLevelLoaded.AddListener(CreatePlayer);

			if (payload == SceneSets.MainMenu)
				_sceneLoader.LoadMainMenu();
			else
			{
				_progressService.SaveLevel(payload);
				_sceneLoader.LoadSceneSet(GetSceneContainer(payload));
			}

			_sceneLoader.OnLoaded.AddListener(GoToGameLoop);
		}

		public void Exit() { }

		private void GoToGameLoop()
		{
			_sceneLoader.OnLoaded.RemoveListener(GoToGameLoop);
			GameStateMachine.Instance.EnterGameLoopState();
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