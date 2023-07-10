using GameCore.GameServices;
using UnityEditor;

namespace GameCore.StateMachine
{
	public sealed class GameStateBootstrap : IGameState
	{
		private readonly GameStateMachine _stateMachine;
		private readonly ServiceLocator _serviceLocator;

		public GameStateBootstrap(GameStateMachine stateMachine, ServiceLocator serviceLocator)
		{
			_stateMachine = stateMachine;
			_serviceLocator = serviceLocator;
		}

		public async void Enter()
		{
			await _serviceLocator.InitServices();
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