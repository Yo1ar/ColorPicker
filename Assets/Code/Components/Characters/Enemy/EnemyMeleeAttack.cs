using System;
using UnityEngine;
using Utils.Constants;
using Cooldown = Utils.Cooldown;

namespace Characters.Enemy
{
	public class EnemyMeleeAttack : MonoBehaviour
	{
		[SerializeField] private GameTag _targetTag;
		[SerializeField] private float _attackCooldownTime;
		private Cooldown _cooldown;
		public event Action OnAttack;

		private void Awake() =>
			SetupCooldown();

		private void OnEnable() =>
			OnAttack += _cooldown.Reset;

		private void OnDisable() =>
			OnAttack -= _cooldown.Reset;

		private void OnTriggerStay2D(Collider2D other)
		{
			PerformAttack(other.gameObject);
		}

		private void SetupCooldown()
		{
			_cooldown = new Cooldown(_attackCooldownTime);
			_cooldown.Reset();
		}
	
		public void PerformAttack(GameObject target)
		{
			if (!ReadyToAttack(target))
				return;

			if (target.TryGetComponent(out IHealth health))
				health.Damage();

			OnAttack?.Invoke();
		}

		private bool ReadyToAttack(GameObject target) =>
			_cooldown.IsReady && target.CompareTag(_targetTag.ToString());
	}
}