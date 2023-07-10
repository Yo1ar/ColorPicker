using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateLoadProgress : IGameState
	{
		private readonly GameStateMachine _stateMachine;

		public GameStateLoadProgress(GameStateMachine stateMachine) => 
			_stateMachine = stateMachine;


		public void Enter() => 
			_stateMachine.EnterLoadLevelState(SceneSets.MainMenu);

		public void Update()
		{
		}

		public void Exit()
		{
		}
	}
}