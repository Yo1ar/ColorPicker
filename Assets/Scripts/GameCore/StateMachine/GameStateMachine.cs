using GameCore.GameServices;
using GameCore.GameUI;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateMachine : Singleton<GameStateMachine>
	{
		private IGameStateExitable _currentState;
		private GameStateBootstrap _stateBootstrap;
		private GameStateLoadProgress _stateLoadProgress;
		private GameStateLoadLevel _stateLoadLevel;
		private GameStateGameLoop _stateGameLoop;
		
		private void Update() =>
			_currentState?.Update();

		public GameStateMachine Init(Services services)
		{
			LoadingScreen loadingScreen = FindLoadingScreen();
			
			_stateBootstrap = new GameStateBootstrap(this, services);
			_stateLoadProgress = new GameStateLoadProgress(this);
			_stateLoadLevel = new GameStateLoadLevel(this, loadingScreen);
			_stateGameLoop = new GameStateGameLoop(this);
			return this;
		}

		private LoadingScreen FindLoadingScreen() => 
			FindObjectOfType<LoadingScreen>();

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

		private bool IsSameState(IGameStateUpdatable newState) =>
			_currentState?.GetType() == newState.GetType();
	}
}