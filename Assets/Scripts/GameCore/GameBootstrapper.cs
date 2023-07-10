using UnityEngine;

namespace GameCore
{
	public sealed class GameBootstrapper : MonoBehaviour
	{
		private Game _game;

		private void Awake()
		{
			StartNewGame();
		}

		private void StartNewGame() =>
			_game = new Game();
	}
}