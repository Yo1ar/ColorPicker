using GameCore.StateMachine;
using Level;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Utils.Constants;

namespace UI
{
	public class GameMenuController : IGameMenuController
	{
		private const string k_gameMenu = "gameMenu";
		private const string k_resumeButton = "btn_resume";
		private const string k_restartButton = "btn_restart";
		private const string k_settingsButton = "btn_settings";
		private const string k_mainMenuButton = "btn_mainMenu";

		private readonly Button _restartButton;
		private readonly Button _mainMenuButton;
		private readonly RespawnPlayerBehaviour _playerRespawn;

		public VisualElement RootElement { get; }
		public Button SettingsButton { get; }
		public Button ResumeButton { get; }

		public GameMenuController(UIDocument document)
		{
			_playerRespawn = GameObject.FindWithTag(Tags.RESPAWN).GetComponent<RespawnPlayerBehaviour>();

			RootElement = document.rootVisualElement.Q<VisualElement>(k_gameMenu);
			ResumeButton = RootElement.GetVisualElement(k_resumeButton).GetButton();
			_restartButton = RootElement.GetVisualElement(k_restartButton).GetButton();
			SettingsButton = RootElement.GetVisualElement(k_settingsButton).GetButton();
			_mainMenuButton = RootElement.GetVisualElement(k_mainMenuButton).GetButton();

			_restartButton.clicked += OnRestartClicked;
			_mainMenuButton.clicked += OnMainMenuClicked;
		}

		public void Dispose()
		{
			_restartButton.clicked -= OnRestartClicked;
			_mainMenuButton.clicked -= OnMainMenuClicked;
		}

		private void OnRestartClicked() =>
			_playerRespawn.KillPlayer();

		private void OnMainMenuClicked() =>
			GameStateMachine.Instance.EnterLoadLevelState(SceneSets.MainMenu);
	}
}