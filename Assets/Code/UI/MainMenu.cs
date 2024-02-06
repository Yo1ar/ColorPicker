namespace UI
{
	public sealed class MainMenu : UiElement
	{
		private IMainMenuController _mainMenuController;
		private ISettingsMenuController _settingsMenuController;
		private ShowHideUiHandler _showHideUiHandlerSettings;
		private ShowHideUiHandler _showHideUiHandlerMainMenu;

		protected override void Awake()
		{
			base.Awake();

			_mainMenuController = new MainMenuController(Document);
			_settingsMenuController = new SettingsMenuController(Document);

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