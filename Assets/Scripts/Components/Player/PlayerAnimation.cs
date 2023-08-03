using UnityEngine;

namespace Components.Player
{
	public class PlayerAnimation : MonoBehaviour
	{
		private Animator _animator;
		private GroundCheck _groundCheck;
		private Rigidbody2D _rigidbody2D;
		private PlayerSkills _playerSkills;
		private PlayerMove _playerMove;
		private PlayerHealth _playerHealth;
		private int _currentClip;
		private bool _cantPlayClips;
		private bool _damageClipPlaying;
	
		private static readonly int RunHash = Animator.StringToHash("anim_hero_run");
		private static readonly int IdleHash = Animator.StringToHash("anim_hero_idle");
		private static readonly int JumpHash = Animator.StringToHash("anim_hero_jump");
		private static readonly int FallHash = Animator.StringToHash("anim_hero_fall");
		private static readonly int PreAttackHash = Animator.StringToHash("anim_hero_preAttack");
		private static readonly int AttackHash = Animator.StringToHash("anim_hero_attack");
		private static readonly int DamagedHash = Animator.StringToHash("anim_hero_damaged");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_groundCheck = GetComponent<GroundCheck>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerSkills = GetComponent<PlayerSkills>();
			_playerMove = GetComponent<PlayerMove>();
			_playerHealth = GetComponent<PlayerHealth>();
		}

		private void OnEnable() =>
			_playerHealth.OnDamage.AddListener(AnimateDamage);

		private void Update()
		{
			if (_damageClipPlaying)
				return;

			if (_playerSkills.IsAttacking)
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
			PlayClip(_groundCheck.IsGrounded ? PreAttackHash : AttackHash);

		public void AnimateAttack() =>
			PlayClip(AttackHash);

		public void DamageAnimFinished() =>
			_damageClipPlaying = false;

		private void AnimateJump() =>
			PlayClip(JumpHash);

		private void AnimateFall() =>
			PlayClip(FallHash);

		private void AnimateRun() =>
			PlayClip(RunHash);

		private void AnimateIdle() =>
			PlayClip(IdleHash);

		private void AnimateDamage()
		{
			_damageClipPlaying = true;
			PlayClip(DamagedHash);
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