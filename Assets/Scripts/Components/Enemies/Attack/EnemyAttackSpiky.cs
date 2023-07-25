using GameCore.GameServices;
using UnityEngine;
using Utils;

namespace Components.Enemies
{
	public sealed class EnemyAttackSpiky : EnemyAttackBase, IAttackBehavior
	{
		[Header("Spiky")] [SerializeField] private int _spikesCount;
		private Projectile _projectile;
		private ProjectilePool _projectilePool;

		protected override void Awake()
		{
			base.Awake();
			
			_projectile = Services.AssetService.PencilProjectile;
			_projectilePool = new ProjectilePool(_projectile);
		}

		public void PerformAttack()
		{
			for (int i = 0; i < _spikesCount; i++)
				LaunchSpike();

			EnemyBehavior.SetAttacking(false);
		}

		private void LaunchSpike() =>
			_projectilePool.LaunchProjectile(
				position: transform.position,
				direction: GetDirection(),
				rotation: Vector3.zero);

		private Vector2 GetDirection()
		{
			Vector2 playerPosition = (Player.position - transform.position).ToVector2().normalized;
			return new Vector2(playerPosition.x + GetDeviation(), playerPosition.y + GetDeviation()).normalized;

			float GetDeviation() =>
				Random.Range(-0.5f, 0.5f);
		}
	}
}