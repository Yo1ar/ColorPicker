using Components;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MAX_HEALTH = 3;
		public int CurrentHealth { get; private set; }

		public UnityEvent OnDamage;
		public UnityEvent OnHeal;
		public UnityEvent OnDeath;
		
		private void Awake() =>
			CurrentHealth = MAX_HEALTH;

		public void Damage()
		{
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
				Die();
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

		private void Die()
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
			Die();
	}
}