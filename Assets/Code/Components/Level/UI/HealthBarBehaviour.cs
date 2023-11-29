using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Level.UI
{
	public class HealthBarBehaviour : MonoBehaviour
	{
		[SerializeField] private Image[] _healthViews;
		private FactoryService _factoryService;
		private PlayerHealth _playerHealth;
	
		private void Awake()
		{
			_factoryService = Services.FactoryService;

			if (_factoryService.Player)
				GetPlayerHealth();
		
			_factoryService.OnPlayerCreated.AddListener(GetPlayerHealth);
		}

		private void GetPlayerHealth()
		{
			_playerHealth = _factoryService.Player.GetComponent<PlayerHealth>();
			_playerHealth.OnDamage.AddListener(SetupHealthUI);
			_playerHealth.OnHeal.AddListener(SetupHealthUI);
		
			SetupHealthUI();
		}

		private void SetupHealthUI()
		{
			foreach (Image image in _healthViews)
				image.enabled = false;

			for (int i = 1; i <= _playerHealth.CurrentHealth; i++)
			{
				var view = _healthViews[i - 1];
				
				view.enabled = true;
				view.transform.DoPop(scaleValue: 1.5f, repeatTimes: 1);
			}
		}
	}
}
