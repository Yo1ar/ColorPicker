using UnityEngine;

namespace Components.Enemies
{
	[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(EnemyBehavior))]
	public class EnemyMove : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private bool _stopWhenAttack;
		[SerializeField] private Transform _visualToRotate;
		private Rigidbody2D _rigidbody2D;
		private int _direction;
		private EnemyBehavior _enemyBehavior;

		private void Awake()
		{
			_enemyBehavior = GetComponent<EnemyBehavior>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			if (_stopWhenAttack && _enemyBehavior.CanAttackPLayer)
			{
				_enemyBehavior.SetMoving(false);
				return;
			}

			Move();
			_enemyBehavior.SetMoving(_rigidbody2D.velocity.x > 0);
		}

		public void SetLookDirection(int direction)
		{
			if (direction == _enemyBehavior.LookDirection)
				return;

			_direction = direction;
			_enemyBehavior.SetDirection(direction);
			
			if (_visualToRotate)
				_visualToRotate.localScale = direction >= 0
					? new Vector3(-1, 1, 1)
					: new Vector3(1, 1, 1);
			else
				transform.localScale = direction >= 0
					? new Vector3(-1, 1, 1)
					: new Vector3(1, 1, 1);
		}

		private void Move() =>
			_rigidbody2D.velocity = new Vector2(_direction * _speed, _rigidbody2D.velocity.y);
	}
}