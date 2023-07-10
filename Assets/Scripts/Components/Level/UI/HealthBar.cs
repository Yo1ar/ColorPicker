namespace Components.Level.UI
{
	using Player;
	using DG.Tweening;
	using DG.Tweening.Core;
	using DG.Tweening.Plugins.Options;
	using UnityEngine;
	using UnityEngine.UI;
	
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Image _back;
		private Slider _healthSlider;
		private PlayerHealth _playerHealth;
		private TweenerCore<Color, Color, ColorOptions> _tween;
		private const Ease Ease = DG.Tweening.Ease.InSine;

		private readonly Color _damageColor = Color.red;
		private readonly Color _healColor = Color.green;
		private const float AnimationDuration = 2f;

		private void Awake()
		{
			_playerHealth = FindFirstObjectByType<PlayerHealth>();
			_healthSlider = GetComponent<Slider>();
		}

		private void OnEnable()
		{
			_playerHealth.OnDamage += OnDamage;
			_playerHealth.OnHeal += OnHeal;
		}

		private void OnDisable()
		{
			_playerHealth.OnDamage -= OnDamage;
			_playerHealth.OnHeal -= OnHeal;
			
			_tween.Kill();
		}

		[ContextMenu("On Damage")]
		private void OnDamage()
		{
			CheckTweenActive();
			SetBarValue();
			StartFadeAnimation(_damageColor);
		}

		[ContextMenu("On Heal")]
		private void OnHeal()
		{
			CheckTweenActive();
			SetBarValue();
			StartFadeAnimation(_healColor);
		}

		private void CheckTweenActive()
		{
			if (_tween is {active: true})
				_tween.Kill();
		}

		private void SetBarValue() => 
			_healthSlider.value = _playerHealth.currentHealth;

		private void StartFadeAnimation(Color color)
		{
			_back.color = color;
			_tween = _back.DOColor(Color.white, AnimationDuration).SetEase(Ease);
		}
	}
}