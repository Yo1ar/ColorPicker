using GameCore.GameServices;
using UnityEngine;
using Utils.Constants;

namespace Components.Enemies
{
	[RequireComponent(typeof(EnemyMove))]
	public class EnemyChasePlayer : MonoBehaviour
	{
		[SerializeField] private float _chaseRadius;
		[SerializeField] private LayerMask _playerLayer;
		private bool _isChasingPlayer;
		private Transform _player;
		private EnemyMove _enemyMove;
		private readonly Collider2D[] _results = new Collider2D[1];

		private void Awake() =>
			_enemyMove = GetComponent<EnemyMove>();

		private void OnEnable() =>
			Services.FactoryService.OnPlayerCreated.AddListener(SetPlayer);

		private void OnDisable() =>
			Services.FactoryService.OnPlayerCreated.RemoveListener(SetPlayer);

		private void FixedUpdate() =>
			_enemyMove.SetLookDirection(IsPlayerInChaseRadius() ? CalculateDirection() : 0);

		private int CalculateDirection()
		{
			float direction = (_player.position - transform.position).normalized.x;

			if (direction > 0)
				return 1;
			else
				return -1;
		}

		private bool IsPlayerInChaseRadius()
		{
			int count = Physics2D.OverlapCircleNonAlloc(transform.position, _chaseRadius, _results, _playerLayer);
			return count > 0;
		}

		private void SetPlayer(Transform player) =>
			_player = player;

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Colors.BlueT;
			Gizmos.DrawWireSphere(transform.position, _chaseRadius);
		}
	}
}