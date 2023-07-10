using UnityEngine;

namespace Components.Enemies
{
	[RequireComponent(typeof(Animator))]
	public sealed class EnemyBehavior : MonoBehaviour
	{
		private readonly int _attackTriggerHash = Animator.StringToHash("attack");
		private readonly int _moveAnimHash = Animator.StringToHash("isMoving");
		private Animator _animator;

		public bool IsAttacking { get; private set; }
		public bool CanAttackPLayer { get; private set; }
		public bool IsMoving { get; private set; }
		public int LookDirection { get; private set; }

		private void Awake() =>
			_animator = GetComponent<Animator>();

		public void SetAttacking(bool value)
		{
			IsAttacking = value;

			if (IsAttacking)
				_animator.SetTrigger(_attackTriggerHash);
		}

		public void SetMoving(bool value)
		{
			IsMoving = value;
			_animator.SetBool(_moveAnimHash, value);
		}

		public void SetCanAttack(bool value) =>
			CanAttackPLayer = value;

		public void SetDirection(int direction) =>
			LookDirection = direction;
	}
}