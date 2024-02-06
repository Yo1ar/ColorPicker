using Characters.Player;
using GameCore.GameServices;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
	public sealed class HealthBarController : IUiController
	{
		private const string k_healthBar = "healthBar";

		private readonly VisualElement[] _visualElements = new VisualElement[3];
		private readonly FactoryService _factoryService;
		private PlayerHealth _playerHealth;

		public VisualElement RootElement { get; }

		public HealthBarController(UIDocument document)
		{
			_factoryService = Services.FactoryService;
			RootElement = document.rootVisualElement.GetVisualElement(k_healthBar);

			for (int i = 0; i < _visualElements.Length; i++)
				_visualElements[i] = RootElement.GetVisualElement($"pen{i + 1}");

			if (_factoryService.Player)
				GetPlayerHealth();

			_factoryService.OnPlayerCreated.AddListener(GetPlayerHealth);
		}

		public void Dispose()
		{
			_factoryService.OnPlayerCreated.RemoveListener(GetPlayerHealth);
			_playerHealth.OnDamage.RemoveListener(SetupHealthUI);
			_playerHealth.OnHeal.RemoveListener(SetupHealthUI);
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
			foreach (VisualElement element in _visualElements)
				element.visible = false;

			for (int i = 1; i <= _playerHealth.CurrentHealth; i++)
				_visualElements[i - 1].visible = true;

			PopHealthBar();
		}

		private void PopHealthBar()
		{
			RootElement.experimental.animation.Scale(0.35f, 100).OnCompleted(PopBack);

			return;

			void PopBack() =>
				RootElement.experimental.animation.Scale(0.3f, 200);
		}
	}
}