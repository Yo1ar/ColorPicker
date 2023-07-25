using GameCore.GameServices;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateLoadProgress : IGameState
	{
		private readonly GameStateMachine _stateMachine;
		private readonly ProgressService _progressService;

		public GameStateLoadProgress(GameStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
			_progressService = Services.ProgressService;
		}

		public void Enter()
		{
			_progressService.LoadProgress();
			_stateMachine.EnterLoadLevelState(SceneSets.MainMenu);
		}

		public void Update()
		{
		}

		public void Exit()
		{
		}
	}
}