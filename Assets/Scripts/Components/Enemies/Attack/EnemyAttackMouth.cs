using System.Threading.Tasks;
using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Components.Enemies
{
	public class EnemyAttackMouth : EnemyAttackBase, IAttackBehavior
	{
		[Header("Mouth")] [SerializeField] private float _spawnHeight;
		[SerializeField] private Vector3 _initialRotation;
		[SerializeField] private float _deviation;
		[SerializeField] private int _projectileCount;
		[SerializeField] private float _projectileDelay;

		private Transform _portalPrefab;
		private Projectile _projectilePrefab;
		private ProjectilePool _projectilePool;
		private Vector3 _playerStaticPosition;
		private Vector3 OverPlayerPosition => new(Player.position.x, _spawnHeight + transform.position.y);
		private Vector3 InstantiatePosition
		{
			get
			{
				float randomDeviation = Random.Range(-_deviation / 2, _deviation / 2);
				Vector3 deviatedPlayerPosition = _playerStaticPosition.AddX(randomDeviation);
				return deviatedPlayerPosition;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			_portalPrefab = Instantiate(Services.AssetService.PortalAttack);
			_projectilePrefab = Services.AssetService.PencilProjectile;
			_projectilePool = new ProjectilePool(_projectilePrefab);
			_portalPrefab.gameObject.SetActive(false);
		}

		public async void PerformAttack()
		{
			_playerStaticPosition = OverPlayerPosition;
			ShowTeleport(_playerStaticPosition);

			for (int i = 0; i < _projectileCount; i++)
			{
				LaunchProjectile();
				await Task.Delay((int)(_projectileDelay * 1000));
			}

			HideTeleport();
		}

		private void LaunchProjectile() =>
			_projectilePool.LaunchProjectile(
				position: InstantiatePosition,
				direction: Vector3.down,
				rotation: _initialRotation);

		private void ShowTeleport(Vector3 position)
		{
			_portalPrefab.position = position;
			_portalPrefab.gameObject.SetActive(true);
		}

		private void HideTeleport() =>
			_portalPrefab.gameObject.SetActive(false);

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			Gizmos.color = Colors.BlueT;

			if (Player)
			{
				Gizmos.DrawCube(
					new Vector3(Player.position.x, _spawnHeight + transform.position.y),
					new Vector3(_deviation, 0.1f));
			}
			else
			{
				Gizmos.DrawCube(
					transform.position + new Vector3(1, _spawnHeight),
					new Vector3(_deviation, 0.1f));
			}
		}
	}
}