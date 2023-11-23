using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MAX_HEALTH = 3;
		public int CurrentHealth { get; private set; }
		public bool IsFullHealth => CurrentHealth == MAX_HEALTH;

		public UnityEvent OnDamage;
		public UnityEvent OnHeal;
		public UnityEvent OnDeath;
		private bool _isInvul;

		private void Awake() =>
			CurrentHealth = MAX_HEALTH;

		public void Anim_InvulOff() =>
			_isInvul = false;

		public void Anim_InvulOn() =>
			_isInvul = true;
		
		public void Damage()
		{
			if (_isInvul)
				return;
			
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