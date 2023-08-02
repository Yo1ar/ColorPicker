using GameCore.GameUI;
using UnityEngine;

namespace GameCore
{
	public sealed class GameBootstrapper : MonoBehaviour
	{
		private Game _game;
		private LoadingScreen _loadingScreen;

		private void Awake()
		{
			_loadingScreen = FindObjectOfType<LoadingScreen>();
			_game = new Game(_loadingScreen);
		}
	}
}