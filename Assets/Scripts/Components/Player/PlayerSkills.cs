using Components.Level;
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

		public bool IsAttacking { get; private set; }

		private void Awake()
		{
			_playerInventory = GetComponent<PlayerInventory>();
			_playerAnimation = GetComponent<PlayerAnimation>();
			_playerHealth = GetComponent<PlayerHealth>();
			
			_projectile = ServiceLocator.assetService.FireballProjectile;
			_fireballPool = new ProjectilePool(_projectile);
		}

		private void OnEnable() =>
			_playerHealth.OnDamage += SetPlayerNotAttacking;

		private void OnDisable() =>
			_playerHealth.OnDamage += SetPlayerNotAttacking;

		public void EraseSomething()
		{
			if (CanErase())
				Debug.Log("Something erased");
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