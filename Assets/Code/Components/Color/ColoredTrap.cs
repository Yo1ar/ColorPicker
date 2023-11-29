using System.Collections;
using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using Utils;

[SelectionBase]
public sealed class ColoredTrap : ColorCheckerBase
{
	[Header("Trap")] [SerializeField] private float _reloadTime = 2.1f;
	[SerializeField] private Sprite _reloadedSprite;
	[SerializeField] private Sprite _hitSprite;
	private PlayerHealth _playerHealth;
	private bool _isReloaded;
	private AudioSourcesController _audioSourcesController;
	private AudioClip _trapSound;
	
	protected override void Awake()
	{
		base.Awake();
		
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		_trapSound = Services.AssetService.SoundsConfig.TrapClip;
	}

	private void Start()
	{
		SetSprite(_reloadedSprite);
		_isReloaded = true;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.TryGetComponent(out ColorHolderBase colorHolder) || 
		    !other.TryGetComponent(out IHealth health))
			return;

		if (IsSameColor(@as: colorHolder) && _isReloaded)
		{
			_audioSourcesController.PlaySoundOneShot(_trapSound);
			health.Damage();
			SetSprite(_hitSprite);
			StartCoroutine(ReloadRoutine());
		}
	}

	private IEnumerator ReloadRoutine()
	{
		if (!_isReloaded)
			yield break;

		_isReloaded = false;

		yield return new WaitForSeconds(_reloadTime);
		SetSprite(_reloadedSprite);
		_isReloaded = true;
	}

	private void SetSprite(Sprite newSprite) =>
		View.sprite = newSprite;
}