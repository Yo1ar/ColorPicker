using System;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public sealed class MainMenuNew : MonoBehaviour
	{
		[SerializeField] private UIDocument _uiDocument;

		private ISettingsMenuController _settingsMenuController;
		private IMainMenuController _mainMenuController;
		private ShowHideUiHandler _showHideUiHandlerSettings;
		private ShowHideUiHandler _showHideUiHandlerMainMenu;
		private ProgressService _progressService;

		private void Awake()
		{
			_progressService = Services.ProgressService;

			_mainMenuController = new MainMenuController(_uiDocument, _progressService);
			_settingsMenuController = new SettingsMenuController(_uiDocument, _progressService);

			_mainMenuController.SettingsButton.RegisterCallback<ClickEvent>(ShowSettings);
			_settingsMenuController.BackButton.RegisterCallback<ClickEvent>(ShowMainMenu);

			_showHideUiHandlerSettings = new ShowHideUiHandler(_settingsMenuController.VisualElement, this, hideImmediate: true);
			_showHideUiHandlerMainMenu = new ShowHideUiHandler(_mainMenuController.VisualElement, this);
		}

		private void Start()
		{
			// ShowMainMenu(new ClickEvent());
		}

		private void ShowMainMenu(ClickEvent evt)
		{
			_showHideUiHandlerSettings.Hide();
			_showHideUiHandlerMainMenu.Show();
		}

		private void ShowSettings(ClickEvent evt)
		{
			_showHideUiHandlerSettings.Show();
			_showHideUiHandlerMainMenu.Hide();
		}

		private void OnDisable()
		{
			_mainMenuController.Dispose();
			_settingsMenuController.Dispose();
		}
	}
}