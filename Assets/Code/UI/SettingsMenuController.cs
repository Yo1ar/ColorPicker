using GameCore.GameServices;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
	public sealed class SettingsMenuController : ISettingsMenuController
	{
		private const string k_settingsMenu = "settingsMenu";
		private const string k_musicVolume = "sli_music";
		private const string k_sfxVolume = "sli_sfx";
		private const string k_backButton = "btn_back";

		private readonly AudioSourcesController _audioSourcesController;
		private readonly ProgressService _progressService;
		private readonly Slider _musicVolume;
		private readonly Slider _sfxVolume;
		private readonly Button _backButton;

		public Button BackButton => _backButton;
		public VisualElement RootElement { get; }

		public SettingsMenuController(UIDocument document)
		{
			_progressService = Services.ProgressService;
			_audioSourcesController = Services.AudioService.AudioSourcesController;

			RootElement = document.rootVisualElement.Q<VisualElement>(k_settingsMenu);
			_musicVolume = RootElement.Q<Slider>(k_musicVolume);
			_sfxVolume = RootElement.Q<Slider>(k_sfxVolume);
			_backButton = RootElement.GetVisualElement(k_backButton).GetButton();

			RegisterCallbacks();
			LoadSettings();
		}

		public void Dispose() =>
			UnregisterCallbacks();

		private void RegisterCallbacks()
		{
			_musicVolume.RegisterValueChangedCallback(SetMusicVolume);
			_sfxVolume.RegisterValueChangedCallback(SetSfxVolume);
			_backButton.RegisterCallback<ClickEvent>(OnBackPressed);
		}

		private void UnregisterCallbacks()
		{
			_musicVolume.UnregisterValueChangedCallback(SetMusicVolume);
			_sfxVolume.UnregisterValueChangedCallback(SetSfxVolume);
			_backButton.UnregisterCallback<ClickEvent>(OnBackPressed);
		}

		private void SetMusicVolume(ChangeEvent<float> evt) =>
			_audioSourcesController.SetMusicVolume(evt.newValue);

		private void SetSfxVolume(ChangeEvent<float> evt) =>
			_audioSourcesController.SetSoundVolume(evt.newValue);

		private void OnBackPressed(ClickEvent evt) =>
			SaveSettingsData();

		private void LoadSettings()
		{
			SettingsData settings = _progressService.SettingsData;

			_musicVolume.value = settings.MusicVolume;
			_sfxVolume.value = settings.SoundVolume;

			_audioSourcesController.SetMusicVolume(settings.MusicVolume);
			_audioSourcesController.SetSoundVolume(settings.SoundVolume);
			_audioSourcesController.PlayMusic();
		}

		private void SaveSettingsData()
		{
			var data = new SettingsData(_musicVolume.value, _sfxVolume.value);
			_progressService.SaveSettings(data);
		}
	}
}