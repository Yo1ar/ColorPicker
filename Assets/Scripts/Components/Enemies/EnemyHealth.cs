using TMPro;
using UnityEngine;

namespace Components.Enemies
{
	public class EnemyHealth : MonoBehaviour, IHealth
	{
		[SerializeField] private int _maxLives;
		private int _currentLives;
		private TMP_Text _text;

		private void Start() =>
			_currentLives = _maxLives;

		public void Damage()
		{
			_currentLives = Mathf.Max(0, --_currentLives);

			if (_currentLives == 0)
				gameObject.SetActive(false);
		}

		public void Heal() =>
			_currentLives = Mathf.Min(_maxLives, ++_currentLives);
	}
}