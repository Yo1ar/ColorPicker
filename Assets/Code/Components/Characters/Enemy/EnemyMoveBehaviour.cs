using UnityEngine;

namespace Characters.Enemy
{
	public sealed class EnemyMoveBehaviour : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private SpriteRenderer _view;
		[SerializeField] private float _speed;

		private Vector3 _moveDirection;
		private float _spriteDirection;

		public void SetDirection(Vector3 direction)
		{
			_moveDirection = direction;

			if (_moveDirection != Vector3.zero)
			{
				_spriteDirection = _moveDirection.x;
				_view.flipX = _spriteDirection < 0;
			}
		}

		private void FixedUpdate() =>
			SetVelocity();

		private void SetVelocity() =>
			_rigidbody2D.velocity = _moveDirection * _speed;
	}
}