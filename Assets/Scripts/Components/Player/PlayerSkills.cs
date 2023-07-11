using System;
using Components.Level;
using Components.Player.Eraser;
using GameCore.GameServices;
using UnityEngine;
using Utils.Constants;

namespace Components.Player
{
	[RequireComponent(typeof(PlayerInventory))]
	public sealed class PlayerSkills : MonoBehaviour
	{
		private Projectile _projectile;
		private ProjectilePool _fireballPool;

		private PlayerInventory _playerInventory;
		private PlayerAnimation _playerAnimation;
		private PlayerHealth _playerHealth;
		private FactoryService _factory;
		private bool _readScreenPouchPosition;

		public bool IsAttacking { get; private set; }

		private void Awake()
		{
			_playerInventory = GetComponent<PlayerInventory>();
			_playerAnimation = GetComponent<PlayerAnimation>();
			_playerHealth = GetComponent<PlayerHealth>();

			_factory = ServiceLocator.factoryService;
			_projectile = ServiceLocator.assetService.FireballProjectile;
			_fireballPool = new ProjectilePool(_projectile);
		}

		private void Update()
		{
			if (!_readScreenPouchPosition)
				return;
			
			if (Input.GetMouseButtonDown(0))
			{
				Camera cam = Camera.main;
				Vector3 mPosition = cam.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit =
					Physics2D.Raycast(mPosition, Vector3.forward, 100f, LayerMask.GetMask("Enemy"));

				if (hit && hit.transform.TryGetComponent(out IErasable erasable))
				{
					erasable.Erase();
					HighlightErasables(false);
				}
			}
		}

		private void OnEnable() =>
			_playerHealth.OnDamage += SetPlayerNotAttacking;

		private void OnDisable() =>
			_playerHealth.OnDamage += SetPlayerNotAttacking;

		public void ActivateErasableMode()
		{
			if (_readScreenPouchPosition || !CanErase())
				return;

			HighlightErasables(true);
		}

		private void HighlightErasables(bool value)
		{
			foreach (IErasable erasable in _factory.Erasables)
				erasable.Highlight(value);
			
			_readScreenPouchPosition = value;
		}

		public void TryLaunchFireball()
		{
			if (!CanLaunchFireball())
				return;

			IsAttacking = true;

			_playerAnimation.AnimatePreAttack();
		}

		public void LaunchFireball()
		{
			_fireballPool.LaunchProjectile(
				position: transform.position,
				direction: GetShootDirection(),
				rotation: Vector3.zero);

			SetPlayerNotAttacking();
		}

		private void SetPlayerNotAttacking() =>
			IsAttacking = false;

		private Vector2 GetShootDirection() =>
			new(transform.localScale.x, 0);

		private bool CanErase() =>
			_playerInventory.TryRemoveItem(CollectableItem.Eraser);

		private bool CanLaunchFireball() =>
			_playerInventory.TryRemoveItem(CollectableItem.Fireball) || IsAttacking;
	}
}