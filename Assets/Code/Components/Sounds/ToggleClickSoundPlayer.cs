using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;

public sealed class ToggleClickSoundPlayer : MonoBehaviour
{
	[SerializeField] private Toggle[] _toggles;
	private AudioClip _clickClip;
	private AudioSourcesController _audioController;

	private void Awake()
	{
		_audioController = Services.AudioService.AudioSourcesController;
		_clickClip = Services.AssetService.SoundsConfig.ClickClip;
		
		FindToggles();
	}

	private void OnEnable()
	{
		foreach (Toggle toggle in _toggles)
		{
			if (!toggle)
				return;
			
			toggle.onValueChanged.AddListener(PlayClick);
		}
	}

	private void OnDisable()
	{
		foreach (Toggle toggle in _toggles)
		{
			if (!toggle)
				return;
			
			toggle.onValueChanged.RemoveListener(PlayClick);
		}
	}

	[ContextMenu("Find toggles")]
	private void FindToggles()
	{
		_toggles = GetComponentsInChildren<Toggle>();

		if (_toggles.Length == 0)
			Debug.LogWarning("Can't find toggles in children");
	}

	private void PlayClick(bool value) =>
		_audioController.PlaySoundOneShot(_clickClip);
}