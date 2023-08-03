using UnityEngine;
using UnityEngine.Events;

namespace Components.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MaxHealth = 3;
		public int CurrentHealth { get; private set; }

		public UnityEvent OnDamage;
		public UnityEvent OnHeal;

		private void Awake() =>
			CurrentHealth = MaxHealth;

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
			{
				CurrentHealth = 0;
				Die();
				return;
			}

			CurrentHealth--;
		}

		private void CalculateHeal()
		{
			if (CurrentHealth + 1 >= MaxHealth)
			{
				CurrentHealth = MaxHealth;
				return;
			}

			CurrentHealth++;
		}

		private void Die()
		{
		}
	}
}