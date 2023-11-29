using System;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace GameCore.GameServices
{
	public class ProgressService : ServiceBase
	{
		private const string LEVEL_CONTAINER_KEY_STRING = "levelContainer";
		private const string SETTINGS_KEY_STRING = "settings";

		public SettingsData SettingsData { get; private set; }
		public SceneSets SavedSceneSet { get; private set; }

		public override async Task InitService()
		{
			Game.GameLogger.GameLog("Initialized", this);
			await Task.CompletedTask;
		}

		public bool HasLevelProgress() =>
			PlayerPrefs.HasKey(LEVEL_CONTAINER_KEY_STRING);

		public void SaveLevel(SceneSets sceneSets) =>
			PlayerPrefs.SetString(LEVEL_CONTAINER_KEY_STRING, MakeDataJson(sceneSets));
		
		public void SaveSettings(SettingsData settings) =>
			PlayerPrefs.SetString(SETTINGS_KEY_STRING, MakeDataJson(settings));

		public void LoadProgress()
		{
			LoadSettings();
			LoadLevel();
		}

		private void LoadSettings()
		{
			string savedSettings = PlayerPrefs.GetString(SETTINGS_KEY_STRING);

			SettingsData = savedSettings.IsEmpty()
				? new SettingsData()
				: FromJson<SettingsData>(savedSettings);
		}

		private void LoadLevel()
		{
			string savedSet = PlayerPrefs.GetString(LEVEL_CONTAINER_KEY_STRING);
			SavedSceneSet = savedSet.IsEmpty()
				? SceneSets.Level1
				: FromJson<SceneSets>(savedSet);
		}

		private string MakeDataJson<T>(T data)
		{
			string json = JsonUtility.ToJson(data);
			return json;
		}

		private T FromJson<T>(string json) =>
			JsonUtility.FromJson<T>(json);
	}

	[Serializable]
	public class SettingsData
	{
		public float MusicVolume;
		public float SoundVolume;
		public bool MusicMute;
		public bool SoundMute;
		
		public SettingsData(float musicVolume = 0.5f, float soundVolume = 0.5f, bool musicMute = false, bool soundMute = false)
		{
			MusicVolume = musicVolume;
			SoundVolume = soundVolume;
			MusicMute = musicMute;
			SoundMute = soundMute;
		}
	}
}