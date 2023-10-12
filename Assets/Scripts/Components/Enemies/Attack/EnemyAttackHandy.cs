using Debug;
using UnityEngine;
using Utils.Constants;

namespace Components.Enemies
{
	[RequireComponent(typeof(EnemyBehavior))]
	public sealed class EnemyAttackHandy : EnemyAttackBase, IAttackBehavior
	{
		[Header("Handy")] [SerializeField] private Vector2 _size;
		private readonly Collider2D[] _results = new Collider2D[1];

		public void PerformAttack(GameObject target)
		{
			if (CheckPlayerReachedByAttack() && Player.TryGetComponent(out IHealth health))
				health.Damage();

			EnemyBehavior.SetAttacking(false);
			Cooldown.Reset();
		}

		private bool CheckPlayerReachedByAttack()
		{
			int count = Physics2D.OverlapBoxNonAlloc(
				GetCenter(),
				_size,
				0,
				_results,
				PlayerLayer);

			return count > 0;
		}

#if UNITY_EDITOR
		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			SceneDebugGizmos.DrawGizmosWireCube(GetCenter(), _size, Colors.RedT);
		}
#endif
	}
}