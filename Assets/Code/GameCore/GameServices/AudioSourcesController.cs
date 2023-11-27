using UnityEngine;
using UnityEngine.Audio;

namespace GameCore.GameServices
{
	public class AudioSourcesController : MonoBehaviour
	{
		[SerializeField] private AudioSource _musicSource;
		[SerializeField] private AudioSource _soundSource;
		[SerializeField] private AudioMixer _mixer;
		private const string MUSIC_VOLUME_PARAMETER = "musicVolume";
		private const string SOUND_VOLUME_PARAMETER = "soundVolume";
		private const float DB_LOW_MARGIN = 60f;

		public void ToggleMuteMusic(bool value) =>
			_musicSource.mute = value;

		public void ToggleMuteSound(bool value) =>
			_soundSource.mute = value;

		public void SetMusicVolume(float value) =>
			_mixer.SetFloat(MUSIC_VOLUME_PARAMETER, LinearToDb(value));

		public void SetSoundVolume(float value) =>
			_mixer.SetFloat(SOUND_VOLUME_PARAMETER, LinearToDb(value));

		public void PlayMusic() =>
			_musicSource.Play();

		public void PlaySoundOneShot(AudioClip clip) =>
			_soundSource.PlayOneShot(clip);

		private float LinearToDb(float value) =>
			value * DB_LOW_MARGIN - DB_LOW_MARGIN;
	}
}