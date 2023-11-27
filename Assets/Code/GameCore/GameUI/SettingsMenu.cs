using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsMenu : MonoBehaviour
{
	[SerializeField] private Slider _musicVolume;
	[SerializeField] private Slider _soundVolume;
	[SerializeField] private Toggle _musicMute;
	[SerializeField] private Toggle _soundMute;
	[SerializeField] private GameMenuButton _backButton;

	private AudioSourcesController _audioSourcesController;
	private ProgressService _progressService;

	private void Awake()
	{
		_progressService = Services.ProgressService;
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		LoadSettings();
	}

	private void OnEnable()
	{
		_musicVolume.onValueChanged.AddListener(_audioSourcesController.SetMusicVolume);
		_soundVolume.onValueChanged.AddListener(_audioSourcesController.SetSoundVolume);
		_musicMute.onValueChanged.AddListener(_audioSourcesController.ToggleMuteMusic);
		_soundMute.onValueChanged.AddListener(_audioSourcesController.ToggleMuteSound);
		_backButton.OnClick.AddListener(SaveSettingsData);
	}

	private void OnDisable()
	{
		_musicVolume.onValueChanged.RemoveListener(_audioSourcesController.SetMusicVolume);
		_soundVolume.onValueChanged.RemoveListener(_audioSourcesController.SetSoundVolume);
		_musicMute.onValueChanged.RemoveListener(_audioSourcesController.ToggleMuteMusic);
		_soundMute.onValueChanged.RemoveListener(_audioSourcesController.ToggleMuteSound);
		_backButton.OnClick.RemoveListener(SaveSettingsData);
	}

	private void LoadSettings()
	{
		SettingsData settings = _progressService.SettingsData;
		
		_musicVolume.value = settings.MusicVolume;
		_soundVolume.value = settings.SoundVolume;
		_musicMute.isOn = settings.MusicMute;
		_soundMute.isOn = settings.SoundMute;

		_audioSourcesController.SetMusicVolume(settings.MusicVolume);
		_audioSourcesController.SetSoundVolume(settings.SoundVolume);
		_audioSourcesController.ToggleMuteMusic(settings.MusicMute);
		_audioSourcesController.ToggleMuteSound(settings.SoundMute);
		_audioSourcesController.PlayMusic();
	}

	private void SaveSettingsData()
	{
		var data = new SettingsData(
			_musicVolume.value,
			_soundVolume.value,
			_musicMute.isOn,
			_soundMute.isOn);
		
		_progressService.SaveSettings(data);
	}
}