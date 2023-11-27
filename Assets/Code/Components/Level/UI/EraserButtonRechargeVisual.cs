using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public sealed class EraserButtonRechargeVisual : MonoBehaviour
{
	[SerializeField] private Image _counterImage;
	private Cooldown _eraserCooldown;

	private void Awake()
	{
		if (Services.FactoryService.Player)
			GetEraserCooldown();

		Services.FactoryService.OnPlayerCreated.AddListener(GetEraserCooldown);
	}

	private void GetEraserCooldown() =>
		_eraserCooldown = Services.FactoryService.Player.GetComponent<IPlayerSkills>().EraserCooldown;

	private void LateUpdate()
	{
		float value = (_eraserCooldown.Value - _eraserCooldown.CurrentValue) / _eraserCooldown.Value;
		_counterImage.fillAmount = value;
	}
}