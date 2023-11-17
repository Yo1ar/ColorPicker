using System;
using UnityEngine;

namespace Components.Characters.Enemy
{
	public class EnemyHealth: MonoBehaviour, IHealth
	{
		[SerializeField] private int _maxHealth;
		public int CurrentHealth { get; set; }
		public Action<int> OnHealthChanged { get; set;} 

		private void Awake() =>
			CurrentHealth = _maxHealth;

		public void Damage()
		{
			CurrentHealth = Mathf.Max(0, CurrentHealth - 1);

			if (CurrentHealth == 0)
				ProcessEnemyDeath();
			
			OnHealthChanged?.Invoke(CurrentHealth);
		}

		public void Heal()
		{
			CurrentHealth = Mathf.Min(CurrentHealth + 1, _maxHealth);
			
			OnHealthChanged?.Invoke(CurrentHealth);
		}

		private void ProcessEnemyDeath()
		{
		}
	}
}