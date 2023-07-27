using System;
using GameCore.GameServices;
using UnityEngine;

namespace Components.Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private float _speed;

		private Rigidbody2D _rigidbody2D;
		private Transform _transform;
		private PlayerSkills _playerSkills;
		private InputService _inputService;
		
		public float Direction { get; private set; }

		private void Awake()
		{
			_inputService = Services.InputService;
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerSkills = GetComponent<PlayerSkills>();
			_transform = transform;
		}

		private void OnEnable() =>
			_inputService.OnMovePressed += SetMoveDirection;

		private void OnDisable() =>
			_inputService.OnMovePressed -= SetMoveDirection;

		private void SetMoveDirection(float direction)
		{
			Direction = direction;
			LookTowardsDirection();
		}

		private void LookTowardsDirection()
		{
			if (Direction < 0)
				_transform.localScale = new Vector3(-1, 1, 1);
			else if (Direction > 0)
				_transform.localScale = new Vector3(1, 1, 1);
		}

		private void FixedUpdate() =>
			Move();

		private void Move()
		{
			if (_playerSkills.IsAttacking)
				_rigidbody2D.velocity = Vector2.zero;

			_rigidbody2D.velocity = NewVelocity();
		}

		private Vector2 NewVelocity() =>
			new(Direction * _speed, _rigidbody2D.velocity.y);
	}
}