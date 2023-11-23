using System.Collections;
using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using Utils;

public sealed class ColoredTrap : ColorCheckerBase
{
	[Header("Trap")]
	[SerializeField] private float _reloadTime = 2;
	[SerializeField] private Sprite _reloadedSprite;
	[SerializeField] private Sprite _hitSprite;
	private PlayerHealth _playerHealth;
	private bool _isReloaded;

	protected override void Awake()
	{
		base.Awake();
		
		if (Services.FactoryService.Player)
			SetPlayerHealth();
		
		Services.FactoryService.OnPlayerCreated.AddListener(SetPlayerHealth);
	}

	private void Start()
	{
		SetSprite(_reloadedSprite);
		_isReloaded = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(@as: PlayerColorHolder) && _isReloaded)
		{
			_playerHealth.Damage();
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

	private void SetPlayerHealth() =>
		_playerHealth = Services.FactoryService.Player.GetComponent<PlayerHealth>();

	private void SetSprite(Sprite newSprite) =>
		View.sprite = newSprite;
}