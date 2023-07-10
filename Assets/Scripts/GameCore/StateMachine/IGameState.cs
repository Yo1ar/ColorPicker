namespace GameCore.StateMachine
{
	public interface IGameStatePayloaded<TPayload>: IGameStateExitable
	{
		public void Enter(TPayload payload);
	}
		
	public interface IGameState : IGameStateExitable
	{
		public void Enter();
	}

	public interface IGameStateExitable : IGameStateUpdatable
	{ public void Exit(); }

	public interface IGameStateUpdatable
	{ public void Update(); }

}