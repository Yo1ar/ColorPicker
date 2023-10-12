using GameCore.GameUI;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateMachine
	{
		private readonly GameStateBootstrap _stateBootstrap;
		private readonly GameStateLoadProgress _stateLoadProgress;
		private readonly GameStateLoadLevel _stateLoadLevel;
		private readonly GameStateGameLoop _stateGameLoop;
		private IGameStateExitable _currentState;
		public static GameStateMachine Instance { get; private set; }

		public GameStateMachine(LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner)
		{
			Instance = this;
			_stateBootstrap = new GameStateBootstrap(this);
			_stateLoadProgress = new GameStateLoadProgress(this);
			_stateLoadLevel = new GameStateLoadLevel(this, loadingScreen, coroutineRunner);
			_stateGameLoop = new GameStateGameLoop(this);
		}

		public void EnterBootstrapState() =>
			EnterState(_stateBootstrap);

		public void EnterLoadProgressState() =>
			EnterState(_stateLoadProgress);

		public void EnterLoadLevelState(SceneSets sceneSet) =>
			EnterStatePayloaded(_stateLoadLevel, sceneSet);

		public void EnterGameLoopState() =>
			EnterState(_stateGameLoop);

		private void EnterState(IGameState newState)
		{
			if (IsSameState(newState))
				return;

			_currentState?.Exit();
			_currentState = newState;
			newState.Enter();
		}

		private void EnterStatePayloaded<TPayload>(IGameStatePayloaded<TPayload> newState, TPayload payload)
		{
			if (IsSameState(newState))
				return;

			_currentState?.Exit();
			_currentState = newState;
			newState.Enter(payload);
		}

		private bool IsSameState(IGameStateExitable newState) =>
			_currentState?.GetType() == newState.GetType();
	}
}