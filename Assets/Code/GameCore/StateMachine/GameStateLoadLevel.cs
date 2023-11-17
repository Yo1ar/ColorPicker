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

		public GameStateLoadLevel(GameStateMachine stateMachine, LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner)
		{
			_stateMachine = stateMachine;
			_sceneLoader = new SceneLoader(loadingScreen, coroutineRunner);
			_factoryService = Services.FactoryService;
			_progressService = Services.ProgressService;
		}

		public void Enter(SceneSets payload)
		{
			if (payload == SceneSets.MainMenu)
				_sceneLoader.Load(SceneSets.MainMenu, GoToGameLoop);
			else
			{
				_progressService.SaveLevel(payload);
				_sceneLoader.Load(payload, PrepareLevel);
			}
		}

		public void Exit() { }

		private void GoToGameLoop() =>
			GameStateMachine.Instance.EnterGameLoopState();

		private void PrepareLevel()
		{
			CreatePlayer();
			GoToGameLoop();
		}

		private void CreatePlayer()
		{
			if (TryFindRespawn(out Transform respawn))
				_factoryService.CreatePlayer(respawn.position);
			else
				UnityEngine.Debug.Log("Can't find player respawn");
		}

		private bool TryFindRespawn(out Transform respawn)
		{
			var respawnObject = GameObject.FindGameObjectWithTag(Tags.RESPAWN);
			respawn = respawnObject.transform;
			return respawnObject != null;
		}
	}
}