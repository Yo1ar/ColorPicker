using GameCore;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
	public sealed class MainMenuController : IMainMenuController
	{
		private const string PLAY_TEXT = "Play";
		private const string CONTINUE_TEXT = "Continue";

		private const string k_mainMenu = "mainMenu";
		private const string k_playButton = "btn_play";
		private const string k_settingsButton = "btn_settings";
		private const string k_creditsButton = "btn_credits";
		private const string k_exitButton = "btn_exit";

		private readonly ProgressService _progressService;
		private readonly Button _playButton;
		private readonly Button _creditsButton;
		private readonly Button _exitButton;

		public Button SettingsButton { get; }
		public VisualElement RootElement { get; }

		public MainMenuController(UIDocument document)
		{
			_progressService = Services.ProgressService;
			RootElement = document.rootVisualElement.Q<VisualElement>(k_mainMenu);

			_playButton = RootElement.GetVisualElement(k_playButton).GetButton();
			SettingsButton = RootElement.GetVisualElement(k_settingsButton).GetButton();
			_creditsButton = RootElement.GetVisualElement(k_creditsButton).GetButton();
			_exitButton = RootElement.GetVisualElement(k_exitButton).GetButton();

			SetPlayButtonInitialText();

			_playButton.RegisterCallback<ClickEvent>(OnClickPlay);
			SettingsButton.RegisterCallback<ClickEvent>(OnClickSettings);
			_creditsButton.RegisterCallback<ClickEvent>(OnClickCredits);
			_exitButton.RegisterCallback<ClickEvent>(OnClickExit);
		}

		public void Dispose()
		{
			_playButton.UnregisterCallback<ClickEvent>(OnClickPlay);
			SettingsButton.UnregisterCallback<ClickEvent>(OnClickSettings);
			_creditsButton.UnregisterCallback<ClickEvent>(OnClickCredits);
			_exitButton.UnregisterCallback<ClickEvent>(OnClickExit);
		}

		public void SetPlayButtonText(string text) =>
			_playButton.text = text;

		private void OnClickPlay(ClickEvent evt) =>
			GameStateMachine.Instance.EnterLoadLevelState(_progressService.SavedSceneSet);

		private void OnClickSettings(ClickEvent evt) { }

		private void OnClickCredits(ClickEvent evt) { }

		private void OnClickExit(ClickEvent evt) =>
			Game.Quit();

		private void SetPlayButtonInitialText() =>
			_playButton.text = _progressService.HasLevelProgress() ? CONTINUE_TEXT : PLAY_TEXT;
	}
}