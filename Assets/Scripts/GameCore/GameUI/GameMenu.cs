using System;
using System.Threading.Tasks;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine;
using Utils.Constants;

namespace GameCore.GameUI
{
	public class GameMenu : MonoBehaviour
	{
		[SerializeField] private GameMenuButton _resumeButton;
		[SerializeField] private GameMenuButton _settingsButton;
		[SerializeField] private GameMenuButton _goToMainButton;
		private ShowHideCanvasGroup _showHideCanvas;
		private InputService _inputSystem;

		private void Awake()
		{
			_inputSystem = Services.InputService;
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();
		}

		private void OnEnable()
		{
			_inputSystem.OnBackPressed += ShowHideGameMenu;
			
			_resumeButton.OnClick += ResumeAction;
			_resumeButton.OnClick += ShowHideGameMenu;

			_settingsButton.OnClick += SettingsAction;
			_settingsButton.OnClick += ShowHideGameMenu;
			
			_goToMainButton.OnClick += MainMenuAction;
			_goToMainButton.OnClick += ShowHideGameMenu;
		}

		private void OnDisable()
		{
			_inputSystem.OnBackPressed -= ShowHideGameMenu;
			
			_resumeButton.OnClick -= ResumeAction;
			_resumeButton.OnClick -= ShowHideGameMenu;
			
			_settingsButton.OnClick -= SettingsAction;
			_settingsButton.OnClick -= ShowHideGameMenu;
			
			_goToMainButton.OnClick -= MainMenuAction;
			_goToMainButton.OnClick -= ShowHideGameMenu;
		}

		private void ShowHideGameMenu()
		{
			if (_showHideCanvas.IsShown)
				Hide();
			else
				Show();
		}

		private void Show()
		{
			_showHideCanvas.Show();
			Game.Pause();
		}

		private void Hide()
		{
			_showHideCanvas.Hide();
			Game.Unpause();
			MakeTextsNormal();
		}

		private async void ResumeAction() =>
			await UnderlineWithDelay(_resumeButton, Hide);

		private async void SettingsAction() => 
			await UnderlineWithDelay(_settingsButton, ShowSettings);

		private async void MainMenuAction() => 
			await UnderlineWithDelay(_goToMainButton, GoToMainMenu);

		private async Task UnderlineWithDelay(GameMenuButton button, Action afterDelay)
		{
			button.Underline();
			await Task.Delay(500);
			afterDelay?.Invoke();
		}

		private void ShowSettings() =>
			Debug.Log("Settings");

		private void GoToMainMenu() => 
			GameStateMachine.instance.EnterLoadLevelState(SceneSets.MainMenu);
		
		private void MakeTextsNormal()
		{
			_resumeButton.MakeTextNormal();
			_settingsButton.MakeTextNormal();
			_goToMainButton.MakeTextNormal();
		}
	}
}