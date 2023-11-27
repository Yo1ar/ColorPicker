using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public sealed class SpeedUpButtonRechargeVisual : MonoBehaviour
{
	[SerializeField] private Image _counterImage;
	
	private Cooldown _speedUpCooldown;

	private void Awake()
	{
		if (Services.FactoryService.Player)
			GetMoveComponent();

		Services.FactoryService.OnPlayerCreated.AddListener(GetMoveComponent);
	}

	private void GetMoveComponent() =>
		_speedUpCooldown = Services.FactoryService.Player.GetComponent<PlayerMove>().SpeedUpCooldown;

	private void LateUpdate()
	{
		float value = (_speedUpCooldown.Value - _speedUpCooldown.CurrentValue) / _speedUpCooldown.Value;
		_counterImage.fillAmount = value;
	}
}