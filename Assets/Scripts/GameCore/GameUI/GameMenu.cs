using System;
using System.Threading.Tasks;
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
		[SerializeField] private float _gap;
		private ShowHideCanvasGroup _showHideCanvas;

		private void Awake() =>
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				ShowHideGameMenu();
		}

		private void ShowHideGameMenu()
		{
			if (_showHideCanvas.IsShown)
				Hide();
			else
				Show();
		}

		private void OnEnable()
		{
			_resumeButton.OnClick += ResumeAction;
			_settingsButton.OnClick += SettingsAction;
			_goToMainButton.OnClick += MainMenuAction;
		}

		private void OnDisable()
		{
			_resumeButton.OnClick -= ResumeAction;
			_settingsButton.OnClick -= SettingsAction;
			_goToMainButton.OnClick -= MainMenuAction;
		}

		public void Show()
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