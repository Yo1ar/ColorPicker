using GameCore.GameServices;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MAX_HEALTH = 3;
		private bool _isInvul;
		private AudioSourcesController _audioSourcesController;
		private AudioClip _hurtSound;

		public UnityEvent OnDamage;
		public UnityEvent OnHeal;
		public UnityEvent OnDeath;

		public bool IsFullHealth => CurrentHealth == MAX_HEALTH;
		public int CurrentHealth { get; private set; }
		
		private void Awake()
		{
			_audioSourcesController = Services.AudioService.AudioSourcesController;
			_hurtSound = Services.AssetService.SoundsConfig.HurtClip;
			
			CurrentHealth = MAX_HEALTH;
		}

		public void Anim_InvulOff() =>
			_isInvul = false;

		public void Anim_InvulOn() =>
			_isInvul = true;
		
		public void Damage()
		{
			if (_isInvul)
				return;

			_audioSourcesController.PlaySoundOneShot(_hurtSound);
			CalculateDamage();
			OnDamage?.Invoke();
		}

		public void Heal()
		{
			CalculateHeal();
			OnHeal?.Invoke();
		}

		private void CalculateDamage()
		{
			if (CurrentHealth - 1 <= 0)
				Kill();
			else
				CurrentHealth--;
		}

		private void CalculateHeal()
		{
			if (CurrentHealth + 1 >= MAX_HEALTH)
			{
				CurrentHealth = MAX_HEALTH;
				return;
			}

			CurrentHealth++;
		}

		public void Kill()
		{
			CurrentHealth = 0;
			OnDeath?.Invoke();
			Destroy(gameObject);
		}

		[ContextMenu("Invoke Damage")]
		private void InvokeDamage() =>
			Damage();

		[ContextMenu("Invoke Heal")]
		private void InvokeHeal() =>
			Heal();

		[ContextMenu("Invoke Death")]
		private void InvokeDeath() =>
			Kill();
	}
}