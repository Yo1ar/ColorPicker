using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Components.Enemies
{
	[RequireComponent(typeof(EnemyBehavior))]
	[AddComponentMenu("")]
	public class EnemyAttackBase : MonoBehaviour
	{
		[Header("Base"), SerializeField] private float _cooldownValue;
		[SerializeField] private float _attackTriggerRadius;
		[SerializeField] protected LayerMask PlayerLayer;
		[SerializeField] protected Vector2 Offset;
		protected Cooldown Cooldown;
		protected Transform Player;
		private readonly Collider2D[] _attackTriggers = new Collider2D[1];
		protected EnemyBehavior EnemyBehavior;
		protected Transform Transform;

		private Vector3 OffsetWithDirection => new(Offset.x * transform.localScale.x, Offset.y);
		protected virtual void Awake()
		{
			Transform = transform;
			EnemyBehavior = GetComponent<EnemyBehavior>();
			Cooldown = new Cooldown(_cooldownValue);
			Cooldown.Reset();
		}

		private void Update()
		{
			if (!IsPlayerInAttackZone() || !CanAttack())
				return;

			EnemyBehavior.SetAttacking(true);
			Cooldown.Reset();
		}

		private void OnEnable() =>
			Services.FactoryService.OnPlayerCreated += SetPlayerWhenCreated;

		private void OnDisable() =>
			Services.FactoryService.OnPlayerCreated -= SetPlayerWhenCreated;

		private bool IsPlayerInAttackZone()
		{
			int count = Physics2D.OverlapCircleNonAlloc(
				GetCenter(),
				_attackTriggerRadius,
				_attackTriggers,
				PlayerLayer);
			
			EnemyBehavior.SetCanAttack(count > 0);
			return count > 0;
		}

		protected Vector3 GetCenter()
		{
#if UNITY_EDITOR_64
			Vector3 center = transform.position.Add(OffsetWithDirection);
#else
			Vector3 center = Transform.position.Add(OffsetWithDirection);
#endif
			return center;
		}

		private void SetPlayerWhenCreated(Transform player) =>
			Player = player;

		private bool CanAttack() =>
			Cooldown.isReady;
		
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Colors.Red;
			Gizmos.DrawWireSphere(GetCenter(), _attackTriggerRadius);
		}
	}
}