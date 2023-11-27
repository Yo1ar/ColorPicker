using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine;

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
			_playButton.OnClick.AddListener(PlayAction);
			_settingsButton.OnClick.AddListener(SettingsAction);
			_creditsButton.OnClick.AddListener(CreditsAction);
			_exitButton.OnClick.AddListener(ExitAction);
		}

		private void OnDisable()
		{
			_playButton.OnClick.RemoveListener(PlayAction);
			_settingsButton.OnClick.RemoveListener(SettingsAction);
			_creditsButton.OnClick.RemoveListener(CreditsAction);
			_exitButton.OnClick.RemoveListener(ExitAction);
		}

		private void PlayAction() =>
			LoadLevel();

		private void SettingsAction() =>
			ShowSettings();

		private void CreditsAction() =>
			ShowCredits();

		private void ExitAction() =>
			Game.Quit();

		private void LoadLevel() =>
			GameStateMachine.Instance.EnterLoadLevelState(_progressService.SavedSceneSet);

		private void ShowSettings() {}

		private void ShowCredits(){}
		

		private void SetPlayText() =>
			_playButton.SetText("Play");

		private void SetContinueText() =>
			_playButton.SetText("Continue");
	}
}