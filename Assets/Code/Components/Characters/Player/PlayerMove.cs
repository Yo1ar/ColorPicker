using System.Collections;
using System.Reflection;
using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Characters.Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private int _speed;
		[SerializeField] private int _speedUpSpeed;
		[SerializeField] private float _speedUpTime;
		[SerializeField] private float _speedUpRechargeTime;

		private AudioSourcesController _audioSourcesController;
		private AudioClip _speedUpSound;
		private Rigidbody2D _rigidbody2D;
		private Transform _transform;
		private IPlayerSkills _playerSkills;
		private ColorHolderBase _playerColorHolder;

		public Cooldown SpeedUpCooldown { get; private set; }
		public float Direction { get; private set; }

		private void Awake()
		{
			_playerColorHolder = GetComponent<ColorHolderBase>();
			_audioSourcesController = Services.AudioService.AudioSourcesController;
			_speedUpSound = Services.AssetService.SoundsConfig.SpeedUpClip;
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerSkills = GetComponent<IPlayerSkills>();
			_transform = transform;
			SpeedUpCooldown = new Cooldown(_speedUpRechargeTime);
		}

		private void OnEnable()
		{
			PlayerEventManager.OnMove.AddListener(SetMoveDirection);
			PlayerEventManager.OnSpeedUp.AddListener(SpeedUpPlayer);
		}

		private void FixedUpdate() =>
			Move();

		private void SpeedUpPlayer()
		{
			if (_playerColorHolder.ColorToCheck != EColors.Blue)
				return;
			
			_audioSourcesController.PlaySoundOneShot(_speedUpSound);
			StartCoroutine(SpeedUp());
		}

		private IEnumerator SpeedUp()
		{
			if (_speed == _speedUpSpeed)
				yield break;

			if (!SpeedUpCooldown.IsReady)
				yield break;

			SpeedUpCooldown.Reset();
			
			int normalSpeed = _speed;
			_speed = _speedUpSpeed;

			yield return new WaitForSeconds(_speedUpTime);

			_speed = normalSpeed;
		}

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