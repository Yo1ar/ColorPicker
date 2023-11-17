using GameCore.Events;
using GameCore.GameServices;

namespace GameCore.StateMachine
{
	public sealed class GameStateGameLoop : IGameState
	{
		private readonly GameStateMachine _stateMachine;

		public GameStateGameLoop(GameStateMachine stateMachine) =>
			_stateMachine = stateMachine;

		public void Enter()
		{
		}

		public void Exit()
		{
			Services.FactoryService.ClearErasables();
			GlobalEventManager.OnLevelLoaded.RemoveAllListeners();
		}
	}
}