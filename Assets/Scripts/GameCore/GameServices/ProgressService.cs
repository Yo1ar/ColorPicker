using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace GameCore.GameServices
{
	public class ProgressService : ServiceBase
	{
		private const string LevelContainerStringKey = "levelContainer";
		private const string SettingsStringKey = "settings";

		public SettingsData SettingsData { get; private set; }
		public SceneSets SavedSceneSet { get; private set; }

		public override async Task InitService() =>
			await Task.Delay(0);

		public bool HasLevelProgress() =>
			PlayerPrefs.HasKey(LevelContainerStringKey);

		public void SaveSettings(SettingsData settings) =>
			PlayerPrefs.SetString(SettingsStringKey, MakeDataJson(settings));

		public void LoadProgress()
		{
			LoadSettings();
			LoadLevel();
		}

		private void LoadLevel()
		{
			string savedSet = PlayerPrefs.GetString(LevelContainerStringKey);
			SavedSceneSet = savedSet.IsEmpty()
				? SceneSets.Level1
				: MakeDataFromJson<SceneSets>(savedSet);
		}

		private void LoadSettings()
		{
			string savedSettings = PlayerPrefs.GetString(SettingsStringKey);

			SettingsData = savedSettings.IsEmpty()
				? new SettingsData(1, 1)
				: MakeDataFromJson<SettingsData>(savedSettings);
		}

		public void SaveLevel(SceneSets sceneSets) =>
			PlayerPrefs.SetString(LevelContainerStringKey, MakeDataJson(sceneSets));

		private string MakeDataJson<T>(T data) =>
			JsonUtility.ToJson(data);

		private T MakeDataFromJson<T>(string json) =>
			JsonUtility.FromJson<T>(json);
	}

	public readonly struct SettingsData
	{
		private readonly float _musicVolume;
		private readonly float _soundVolume;

		public float MusicVolume => _musicVolume;
		public float SoundVolume => _soundVolume;

		public SettingsData(float musicVolume, float soundVolume)
		{
			_musicVolume = musicVolume;
			_soundVolume = soundVolume;
		}
	}
}