using GameCore.Events;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace GameCore.GameUI
{
	public sealed class GameMenuNew : MonoBehaviour
	{
		[SerializeField] private UIDocument _document;

		private IGameMenuController _gameMainMenuController;
		private ISettingsMenuController _settingsMenuController;
		private ShowHideUiHandler _showHideUiHandlerWholeMenu;
		private ShowHideUiHandler _showHideUiHandlerGameMenu;
		private ShowHideUiHandler _showHideUiHandlerSettings;
		private VisualElement _root;

		private void Awake()
		{
			_root = _document.rootVisualElement.GetVisualElement("root");
			_gameMainMenuController = new GameMenuController(_document);
			_settingsMenuController = new SettingsMenuController(_document);

			_showHideUiHandlerWholeMenu = new ShowHideUiHandler(_root);
			_showHideUiHandlerGameMenu = new ShowHideUiHandler(_gameMainMenuController.RootElement);
			_showHideUiHandlerSettings = new ShowHideUiHandler(_settingsMenuController.RootElement);

			_showHideUiHandlerWholeMenu.Hide();
			_showHideUiHandlerSettings.Hide();
		}

		private void OnEnable()
		{
			GlobalEventManager.OnBackPressed.AddListener(ToggleWholeMenu);

			_gameMainMenuController.ResumeButton.clicked += HideWholeMenu;
			_gameMainMenuController.SettingsButton.clicked += ShowSettings;
			_settingsMenuController.BackButton.clicked += ShowGameMenu;
		}

		private void OnDisable()
		{
			GlobalEventManager.OnBackPressed.RemoveListener(ToggleWholeMenu);

			_gameMainMenuController.ResumeButton.clicked -= HideWholeMenu;
			_gameMainMenuController.SettingsButton.clicked -= ShowSettings;
			_settingsMenuController.BackButton.clicked -= ShowGameMenu;

			_gameMainMenuController.Dispose();
			_settingsMenuController.Dispose();
		}

		private void ToggleWholeMenu()
		{
			if (_showHideUiHandlerWholeMenu.IsShown)
				HideWholeMenu();
			else
				ShowWholeMenu();
		}

		private void HideWholeMenu()
		{
			Game.SetPause(false);
			_showHideUiHandlerWholeMenu.Hide();
		}

		private void ShowWholeMenu()
		{
			Game.SetPause(true);
			_showHideUiHandlerWholeMenu.Show();
		}

		private void ShowGameMenu()
		{
			_showHideUiHandlerSettings.Hide();
			_showHideUiHandlerGameMenu.Show();
		}

		private void ShowSettings()
		{
			_showHideUiHandlerSettings.Show();
			_showHideUiHandlerGameMenu.Hide();
		}
	}
}