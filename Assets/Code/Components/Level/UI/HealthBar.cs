using Characters.Player;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;

namespace Level.UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Image _back;
		private Slider _healthSlider;
		private PlayerHealth _playerHealth;
		private TweenerCore<Color, Color, ColorOptions> _tween;
		private FactoryService _factoryService;

		private readonly Color _damageColor = Color.red;
		private readonly Color _healColor = Color.green;
		private const float ANIMATION_DURATION = 2f;
		private const Ease EASE = DG.Tweening.Ease.InSine;

		private void Awake()
		{
			_factoryService = Services.FactoryService;
			_healthSlider = GetComponent<Slider>();

			if (_factoryService.Player != null)
				InitPlayer();
			else
				_factoryService.OnPlayerCreated.AddListener(InitPlayer);
		}

		private void OnDisable()
		{
			_factoryService.OnPlayerCreated.RemoveListener(InitPlayer);
			_tween.Kill();
		}

		private void InitPlayer()
		{
			_playerHealth = _factoryService.Player.GetComponent<PlayerHealth>();
			_playerHealth.OnDamage.AddListener(OnDamage);
			_playerHealth.OnHeal.AddListener(OnHeal);
		}

		private void OnDamage()
		{
			CheckTweenActive();
			SetBarValue();
			StartFadeAnimation(_damageColor);
		}

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
			_healthSlider.value = _playerHealth.CurrentHealth;

		private void StartFadeAnimation(Color color)
		{
			_back.color = color;
			_tween = _back.DOColor(Color.white, ANIMATION_DURATION).SetEase(EASE);
		}
	}
}