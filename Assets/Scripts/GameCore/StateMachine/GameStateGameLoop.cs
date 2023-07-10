namespace GameCore.StateMachine
{
	public sealed class GameStateGameLoop : IGameState
	{
		private readonly GameStateMachine _stateMachine;

		public GameStateGameLoop(GameStateMachine stateMachine) =>
			_stateMachine = stateMachine;

		public void Exit()
		{
		}

		public void Update()
		{
		}

		public void Enter()
		{
		}
	}
}