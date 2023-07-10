using System;
using UnityEngine;

namespace Components.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
	{
		private const int MaxHealth = 3;
		public int currentHealth { get; private set; }

		public event Action OnDamage;
		public event Action OnHeal;

		private void Awake() =>
			currentHealth = MaxHealth;
		
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
			if (currentHealth - 1 <= 0)
			{
				currentHealth = 0;
				Die();
				return;
			}

			currentHealth--;
		}

		private void CalculateHeal()
		{
			if (currentHealth + 1 >= MaxHealth)
			{
				currentHealth = MaxHealth;
				return;
			}

			currentHealth++;
		}

		private void Die() =>
			Debug.Log("GAME OVER!");
	}
}