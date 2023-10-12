using UnityEngine;
using UnityEngine.Events;

namespace Components.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MAX_HEALTH = 3;
		public int CurrentHealth { get; private set; }

		public UnityEvent OnDamage;
		public UnityEvent OnHeal;

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
			{
				CurrentHealth = 0;
				Die();
				return;
			}

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
		}
	}
}