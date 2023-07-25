using GameCore.GameServices;

namespace GameCore.StateMachine
{
	public sealed class GameStateBootstrap : IGameState
	{
		private readonly GameStateMachine _stateMachine;
		private readonly Services _services;

		public GameStateBootstrap(GameStateMachine stateMachine, Services services)
		{
			_stateMachine = stateMachine;
			_services = services;
		}

		public async void Enter()
		{
			await _services.InitServices();
			_stateMachine.EnterLoadProgressState();
		}

		public void Update()
		{
		}

		public void Exit()
		{
		}
	}
}