using GameCore.GameServices;
using UnityEngine;
using Utils;
using Cooldown = Utils.Cooldown;

namespace Characters.Enemy
{
	public class EnemyMeleeAttack : MonoBehaviour
	{
		[SerializeField] private ColorCheckerBase _colorChecker;
		[SerializeField] private Animator _animator;
		[SerializeField] private float _attackCooldownTime;
		
		private readonly int _attackHash = Animator.StringToHash("attack");
		private Cooldown _cooldown;
		private ColorHolderBase _playerColorHolder;
		private IHealth _playerHealth;
		private AudioSourcesController _audioSourcesController;
		private AudioClip _explosionSound;
		
		private void Awake()
		{
			_audioSourcesController = Services.AudioService.AudioSourcesController;
			_explosionSound = Services.AssetService.SoundsConfig.ExplosionClip;
			_cooldown = new Cooldown(_attackCooldownTime);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;
			
			if (!other.TryGetComponent(out _playerColorHolder)||
			    !_colorChecker.IsSameColor(_playerColorHolder) )
				return;
			
			if (!other.TryGetComponent(out _playerHealth))
				return;
			
			if (!_cooldown.IsReady)
				return;

			_animator.Play(_attackHash);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			if (!_colorChecker.IsSameColor(_playerColorHolder))
				return;

			if (_playerHealth == null)
				return;

			if (_cooldown.IsReady)
				_animator.Play(_attackHash);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			_playerHealth = null;
		}

		private void Anim_Attack()
		{
			if (_playerHealth != null)
			{
				_audioSourcesController.PlaySoundOneShot(_explosionSound);
				
				_playerHealth.Damage();
				_cooldown.Reset();
			}
		}
	}
}