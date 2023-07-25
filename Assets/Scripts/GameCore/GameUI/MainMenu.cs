using System;
using System.Threading.Tasks;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine;
using Utils.Constants;

namespace GameCore.GameUI
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private GameMenuButton _playButton;
		[SerializeField] private GameMenuButton _settingsButton;
		[SerializeField] private GameMenuButton _creditsButton;
		[SerializeField] private GameMenuButton _exitButton;

		private ShowHideCanvasGroup _showHideCanvas;
		private ProgressService _progressService;

		private void Awake()
		{
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();
			_progressService = Services.ProgressService;
		}

		private void Start()
		{
			if (_progressService.HasLevelProgress())
				SetContinueText();
			else
				SetPlayText();
		}

		private void OnEnable()
		{
			_playButton.OnClick += PlayAction;
			_settingsButton.OnClick += SettingsAction;
			_creditsButton.OnClick += CreditsAction;
			_exitButton.OnClick += ExitAction;
		}

		private void OnDisable()
		{
			_playButton.OnClick -= PlayAction;
			_settingsButton.OnClick -= SettingsAction;
			_creditsButton.OnClick -= CreditsAction;
			_exitButton.OnClick -= ExitAction;
		}

		public void Show() =>
			_showHideCanvas.Show();

		public void Hide() =>
			_showHideCanvas.Hide();

		private async void PlayAction() =>
			await UnderlineWithDelayButton(_playButton, LoadLevel);

		private async void SettingsAction() =>
			await UnderlineWithDelayButton(_settingsButton, ShowSettings);

		private async void CreditsAction() =>
			await UnderlineWithDelayButton(_creditsButton, ShowCredits);

		private async void ExitAction() =>
			await UnderlineWithDelayButton(_exitButton, Game.Quit);

		private void LoadLevel() =>
			GameStateMachine.instance.EnterLoadLevelState(_progressService.SavedSceneSet);

		private void ShowSettings() =>
			Debug.Log("Settings");

		private void ShowCredits() =>
			Debug.Log("Created by: Yolar Games\n" +
			          "Developer: Nikita Ivanin\n" +
			          "With inspiration from his lovely wife Margarita Wise");

		private async Task UnderlineWithDelayButton(GameMenuButton button, Action afterDelay)
		{
			button.Underline();
			button.enabled = false;

			await Task.Delay(500);
			afterDelay?.Invoke();
		}

		private void SetPlayText() =>
			_playButton.SetText("Play");

		private void SetContinueText() =>
			_playButton.SetText("Continue");
	}
}