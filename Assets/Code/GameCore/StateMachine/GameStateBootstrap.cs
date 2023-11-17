namespace GameCore.StateMachine
{
	public sealed class GameStateBootstrap : IGameState
	{
		private readonly GameStateMachine _stateMachine;

		public GameStateBootstrap(GameStateMachine stateMachine) =>
			_stateMachine = stateMachine;

		public void Enter() =>
			_stateMachine.EnterLoadProgressState();

		public void Exit()
		{
		}
	}
}