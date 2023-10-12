using UnityEngine;

public class MoveComponent : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _moveDirection;
	
	public float MoveDirection
	{
		get => _moveDirection;
		set => _moveDirection = Mathf.Clamp(value, -1f, 1f);
	}

	private void Awake()
	{
		if (!_rigidbody2D)
			UnityEngine.Debug.LogError("Rigidbody2D is empty", this);
	}

	private void FixedUpdate() =>
		Move();

	private void Move()
	{
		Vector2 currentVelocity = _rigidbody2D.velocity;
		_rigidbody2D.velocity = new Vector2(MoveDirection * _moveSpeed, currentVelocity.y);
	}
}