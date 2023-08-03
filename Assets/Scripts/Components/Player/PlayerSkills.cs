using Components.Level;
using Components.Player.Eraser;
using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;

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
		private bool _erasableMode;
		private bool _erasablesHighlighted;
		private Camera _camera;

		public bool IsAttacking { get; private set; }

		private void Awake()
		{
			_playerInventory = GetComponent<PlayerInventory>();
			_playerAnimation = GetComponent<PlayerAnimation>();
			_playerHealth = GetComponent<PlayerHealth>();
			_camera = Camera.main;

			_factory = Services.FactoryService;
			_projectile = Services.AssetService.FireballProjectile;
			_fireballPool = new ProjectilePool(_projectile);
		}

		private void OnEnable() =>
			_playerHealth.OnDamage.AddListener(SetPlayerNotAttacking);

		private void EraseObject()
		{
			if (!_erasableMode)
				return;

			Vector3 mPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit =
				Physics2D.Raycast(mPosition, Vector3.forward, 100f, LayerMask.GetMask("Enemy"));

			if (hit
			    && hit.transform.TryGetComponent(out IErasable erasable)
			    && TryErase())
			{
				erasable.Erase();
				SwitchErasableMode();
			}
		}

		public void SwitchErasableMode()
		{
			if (_erasableMode)
			{
				_erasableMode = !_erasableMode;
				HighlightErasables(_erasableMode);
			}
			else if (CanErase())
			{
				_erasableMode = !_erasableMode;
				HighlightErasables(_erasableMode);
			}
		}

		private void HighlightErasables(bool value)
		{
			foreach (IErasable erasable in _factory.Erasables)
				erasable.Highlight(value);

			if (_erasableMode)
				GlobalEventManager.OnScreenTap.AddListener(EraseObject);
			else
				GlobalEventManager.OnScreenTap.RemoveListener(EraseObject);
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
			_playerInventory.HaveEraser();

		private bool TryErase() =>
			_playerInventory.TryRemoveItem(CollectableItem.Eraser);

		private bool CanLaunchFireball() =>
			_playerInventory.TryRemoveItem(CollectableItem.Fireball) || IsAttacking;
	}
}