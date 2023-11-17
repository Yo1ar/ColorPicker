using System;
using System.Threading.Tasks;
using GameCore.Events;
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
		
		private void Awake() =>
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();

		private void OnEnable()
		{
			GlobalEventManager.OnBackPressed.AddListener(ShowHideGameMenu);
			
			_resumeButton.OnClick.AddListener(ResumeAction);
			_resumeButton.OnClick.AddListener(ShowHideGameMenu);

			_settingsButton.OnClick.AddListener(SettingsAction);
			_settingsButton.OnClick.AddListener(ShowHideGameMenu);
			
			_goToMainButton.OnClick.AddListener(MainMenuAction);
			_goToMainButton.OnClick.AddListener(ShowHideGameMenu);
		}

		private void OnDisable()
		{
			GlobalEventManager.OnBackPressed.RemoveListener(ShowHideGameMenu);
			
			_resumeButton.OnClick.RemoveListener(ResumeAction);
			_resumeButton.OnClick.RemoveListener(ShowHideGameMenu);

			_settingsButton.OnClick.RemoveListener(SettingsAction);
			_settingsButton.OnClick.RemoveListener(ShowHideGameMenu);
			
			_goToMainButton.OnClick.RemoveListener(MainMenuAction);
			_goToMainButton.OnClick.RemoveListener(ShowHideGameMenu);
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
			Game.SetPause(true);
		}

		private void Hide()
		{
			_showHideCanvas.Hide();
			_showHideCanvas.OnHided.AddListener(EndHiding);
		}

		private void EndHiding()
		{
			_showHideCanvas.OnHided.RemoveListener(EndHiding);
			Game.SetPause(false);
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
			UnityEngine.Debug.Log("Settings");

		private void GoToMainMenu() => 
			GameStateMachine.Instance.EnterLoadLevelState(SceneSets.MainMenu);
		
		private void MakeTextsNormal()
		{
			_resumeButton.MakeTextNormal();
			_settingsButton.MakeTextNormal();
			_goToMainButton.MakeTextNormal();
		}
	}
}