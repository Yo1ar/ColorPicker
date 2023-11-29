using GameCore.GameServices;
using UnityEngine;
using Utils;

public sealed class ColorMarker : ColorCheckerBase
{
	private AudioSourcesController _audioSourcesController;
	private AudioClip _sound;

	protected override void Awake()
	{
		base.Awake();
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		_sound = Services.AssetService.SoundsConfig.MarkerClip;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(@as: PlayerColorHolder))
			return;

		_audioSourcesController.PlaySoundOneShot(_sound);
		PlayerColorHolder.SetColor(Color);
	}
}