using Configs;
using GameCore.GameUI;
using UnityEngine;

namespace GameCore
{
	public sealed class GameBootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private GameSetupParameters _editorGameParameters;
		[SerializeField] private GameSetupParameters _clientParameters; // #if

		private Game _game;
		private LoadingScreen _loadingScreen;
		private GameSetupParameters _gameSetupParameters;

		private void Awake()
		{
#if UNITY_EDITOR
			_gameSetupParameters = _editorGameParameters;
#else
			_gameSetupParameters = _clientParameters;
#endif

			_loadingScreen = FindObjectOfType<LoadingScreen>();
			_game = new Game(_gameSetupParameters, _loadingScreen, this);
		}
	}
}