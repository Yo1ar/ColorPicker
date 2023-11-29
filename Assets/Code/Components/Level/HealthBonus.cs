using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using Utils;

namespace Level
{
	[RequireComponent(typeof(CircleCollider2D))]
	public sealed class HealthBonus : ColorCheckerBase
	{
		private AudioSourcesController _audioSourcesController;
		private AudioClip _healSound;

		protected override void Awake()
		{
			base.Awake();
			_audioSourcesController = Services.AudioService.AudioSourcesController;
			_healSound = Services.AssetService.SoundsConfig.GetBonusClip;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			if (!other.TryGetComponent(out PlayerHealth playerHealth))
				return;

			if (playerHealth.IsFullHealth)
				return;

			_audioSourcesController.PlaySoundOneShot(_healSound);
			playerHealth.Heal();
			gameObject.SetActive(false);
		}
	}
}