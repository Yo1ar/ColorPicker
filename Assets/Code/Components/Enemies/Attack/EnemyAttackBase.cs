using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Enemies.Attack
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
		private FactoryService _factoryService;

		private Vector3 OffsetWithDirection => new(Offset.x * transform.localScale.x, Offset.y);

		protected virtual void Awake()
		{
			_factoryService = Services.FactoryService;
			Transform = transform;
			EnemyBehavior = GetComponent<EnemyBehavior>();
			Cooldown = new Cooldown(_cooldownValue);
			Cooldown.Reset();

			if (_factoryService.Player != null)
				InitPlayer();
			else
				_factoryService.OnPlayerCreated.AddListener(InitPlayer);
		}

		private void InitPlayer() =>
			Player = _factoryService.Player.transform;

		private void Update()
		{
			if (!IsPlayerInAttackZone() || !CanAttack())
				return;

			EnemyBehavior.SetAttacking(true);
			Cooldown.Reset();
		}

		protected virtual void OnDisable() =>
			Services.FactoryService.OnPlayerCreated.RemoveListener(InitPlayer);

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
			Cooldown.IsReady;

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected() =>
			SceneDebugGizmos.DrawGizmosWireSphere(GetCenter(), _attackTriggerRadius, Colors.Red);
#endif // UNITY_EDITOR
	}
}