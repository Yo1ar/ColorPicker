using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public sealed class MainMenuNew : MonoBehaviour
	{
		[SerializeField] private UIDocument _uiDocument;

		private IMainMenuController _mainMenuController;
		private ISettingsMenuController _settingsMenuController;
		private ShowHideUiHandler _showHideUiHandlerSettings;
		private ShowHideUiHandler _showHideUiHandlerMainMenu;

		private void Awake()
		{
			_mainMenuController = new MainMenuController(_uiDocument);
			_settingsMenuController = new SettingsMenuController(_uiDocument);

			_showHideUiHandlerMainMenu = new ShowHideUiHandler(_mainMenuController.RootElement);
			_showHideUiHandlerSettings = new ShowHideUiHandler(_settingsMenuController.RootElement);

			_showHideUiHandlerSettings.Hide();
			_showHideUiHandlerMainMenu.Show();
		}

		private void OnEnable()
		{
			_mainMenuController.SettingsButton.clicked += ShowSettings;
			_settingsMenuController.BackButton.clicked += ShowMainMenu;
		}

		private void OnDisable()
		{
			_mainMenuController.SettingsButton.clicked -= ShowSettings;
			_settingsMenuController.BackButton.clicked -= ShowMainMenu;

			_mainMenuController.Dispose();
			_settingsMenuController.Dispose();
		}

		private void ShowMainMenu()
		{
			_showHideUiHandlerSettings.Hide();
			_showHideUiHandlerMainMenu.Show();
		}

		private void ShowSettings()
		{
			_showHideUiHandlerMainMenu.Hide();
			_showHideUiHandlerSettings.Show();
		}
	}
}