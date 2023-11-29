using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemy
{
	public class EnemyHealth : MonoBehaviour, IHealth
	{
		private const int MAX_LIVES = 1;
		private int _currentLives;
		private TMP_Text _text;
		
		public UnityEvent<int> OnDamaged;
		public UnityEvent<int> OnHealed;
		
		private void Start() =>
			_currentLives = MAX_LIVES;

		public void Damage()
		{
			_currentLives = Mathf.Max(0, --_currentLives);
			
			OnDamaged?.Invoke(_currentLives);

			if (_currentLives == 0)
				Kill();
		}

		public void Heal()
		{
			_currentLives = Mathf.Min(MAX_LIVES, ++_currentLives);
			OnHealed?.Invoke(_currentLives);
		}

		public void Kill() =>
			gameObject.SetActive(false);
	}
}