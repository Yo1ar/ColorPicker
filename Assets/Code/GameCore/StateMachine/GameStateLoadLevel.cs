using GameCore.GameServices;
using GameCore.GameUI;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateLoadLevel : IGameStatePayloaded<SceneSets>
	{
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly ProgressService _progressService;

		public GameStateLoadLevel(GameStateMachine stateMachine, LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner)
		{
			_stateMachine = stateMachine;
			_sceneLoader = new SceneLoader(loadingScreen, coroutineRunner);
			_progressService = Services.ProgressService;
		}

		public void Enter(SceneSets payload)
		{
			_sceneLoader.Load(payload, GoToGameLoop);
			
			if (payload != SceneSets.MainMenu)
				_progressService.SaveLevel(payload);
		}

		public void Exit() { }

		private void GoToGameLoop() =>
			GameStateMachine.Instance.EnterGameLoopState();
	}
}