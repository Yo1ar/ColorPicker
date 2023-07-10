using TMPro;
using UnityEngine;

namespace Components.Enemies
{
	public class EnemyHealth : MonoBehaviour, IHealth
	{
		[SerializeField] private int _maxLives;
		private int _currentLives;
		private HealthLabel _healthLabel;
		private TMP_Text _text;

		public int CurrentLives {
			get => _currentLives;
			set {
				_currentLives = value;
				SetLabelLives(value);
			}
		}

		private void Awake()
		{
			_healthLabel = GetComponentInChildren<HealthLabel>();
			if (_healthLabel == null)
				Debug.LogError("Cant find HealthLabel", this);
		}

		private void Start() =>
			CurrentLives = _maxLives;

		public void Damage()
		{
			CurrentLives = Mathf.Max(0, --_currentLives);

			if (_currentLives == 0)
				gameObject.SetActive(false);
		}

		public void Heal() =>
			CurrentLives = Mathf.Min(_maxLives, ++_currentLives);

		private void SetLabelLives(int lives) =>
			_healthLabel.SetLives(lives);
	}
}