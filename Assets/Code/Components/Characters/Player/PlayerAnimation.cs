using UnityEngine;

namespace Characters.Player
{
	public class PlayerAnimation : MonoBehaviour
	{
		private Animator _animator;
		private GroundCheck _groundCheck;
		private Rigidbody2D _rigidbody2D;
		private PlayerController _playerController;
		private PlayerMove _playerMove;
		private PlayerHealth _playerHealth;
		private int _currentClip;
		private bool _cantPlayClips;
		private bool _damageClipPlaying;
	
		private static readonly int _runHash = Animator.StringToHash("anim_hero_run");
		private static readonly int _idleHash = Animator.StringToHash("anim_hero_idle");
		private static readonly int _jumpHash = Animator.StringToHash("anim_hero_jump");
		private static readonly int _fallHash = Animator.StringToHash("anim_hero_fall");
		private static readonly int _preAttackHash = Animator.StringToHash("anim_hero_preAttack");
		private static readonly int _attackHash = Animator.StringToHash("anim_hero_attack");
		private static readonly int _damagedHash = Animator.StringToHash("anim_hero_damaged");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_groundCheck = GetComponent<GroundCheck>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerController = GetComponent<PlayerController>();
			_playerMove = GetComponent<PlayerMove>();
			_playerHealth = GetComponent<PlayerHealth>();
		}

		private void OnEnable() =>
			_playerHealth.OnDamage.AddListener(AnimateDamage);

		private void Update()
		{
			if (_damageClipPlaying)
				return;

			if (_playerController.IsAttacking)
				return;

			PlayMovementAnimations();
		}

		private void PlayMovementAnimations()
		{
			if (_groundCheck.IsGrounded)
				PlayRunIdleAnimation();
			else
				PlayJumpFallAnimation();
		}

		private void PlayRunIdleAnimation()
		{
			if (_playerMove.Direction == 0)
				AnimateIdle();
			else
				AnimateRun();
		}

		private void PlayJumpFallAnimation()
		{
			if (_rigidbody2D.velocity.y > 0)
				AnimateJump();
			if (_rigidbody2D.velocity.y < 0)
				AnimateFall();
		}

		public void AnimatePreAttack() =>
			PlayClip(_groundCheck.IsGrounded ? _preAttackHash : _attackHash);

		public void AnimateAttack() =>
			PlayClip(_attackHash);

		public void DamageAnimFinished() =>
			_damageClipPlaying = false;

		private void AnimateJump() =>
			PlayClip(_jumpHash);

		private void AnimateFall() =>
			PlayClip(_fallHash);

		private void AnimateRun() =>
			PlayClip(_runHash);

		private void AnimateIdle() =>
			PlayClip(_idleHash);

		private void AnimateDamage()
		{
			_damageClipPlaying = true;
			PlayClip(_damagedHash);
		}

		private void PlayClip(int clipHash)
		{
			if (_currentClip == clipHash)
				return;

			_animator.Play(clipHash);
			_currentClip = clipHash;
		}
	}
}