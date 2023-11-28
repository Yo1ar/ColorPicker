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

		private void Start() =>
			Hide();

		private void OnEnable()
		{
			GlobalEventManager.OnBackPressed.AddListener(ShowHideGameMenu);
			
			_resumeButton.OnClick.AddListener(ResumeAction);
			_resumeButton.OnClick.AddListener(ShowHideGameMenu);

			_goToMainButton.OnClick.AddListener(MainMenuAction);
			_goToMainButton.OnClick.AddListener(ShowHideGameMenu);
		}

		private void OnDisable()
		{
			GlobalEventManager.OnBackPressed.RemoveListener(ShowHideGameMenu);
			
			_resumeButton.OnClick.RemoveListener(ResumeAction);
			_resumeButton.OnClick.RemoveListener(ShowHideGameMenu);

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

		private void ResumeAction() =>
			Hide();

		private void MainMenuAction() =>
			GameStateMachine.Instance.EnterLoadLevelState(SceneSets.MainMenu);

		private void MakeTextsNormal()
		{
			_resumeButton.MakeTextNormal();
			_settingsButton.MakeTextNormal();
			_goToMainButton.MakeTextNormal();
		}
	}
}