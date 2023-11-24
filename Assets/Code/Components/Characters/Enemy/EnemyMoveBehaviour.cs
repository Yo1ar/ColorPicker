using UnityEngine;

public sealed class EnemyMoveBehaviour : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private SpriteRenderer _view;
	[SerializeField] private float _speed;

	public Vector3 Direction { get; private set; }

	public void SetDirection(Vector3 direction) =>
		Direction = direction;

	private void FixedUpdate() =>
		SetVelocity();

	private void SetVelocity() =>
		_rigidbody2D.velocity = Direction * _speed;
}