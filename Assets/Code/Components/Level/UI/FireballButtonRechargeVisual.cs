using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public sealed class FireballButtonRechargeVisual : MonoBehaviour
{
	[SerializeField] private Image _counterImage;
	
	private Cooldown _fireballCooldown;

	private void Awake()
	{
		if (Services.FactoryService.Player)
			GetFireballCooldown();

		Services.FactoryService.OnPlayerCreated.AddListener(GetFireballCooldown);
	}

	private void GetFireballCooldown() =>
		_fireballCooldown = Services.FactoryService.Player.GetComponent<IPlayerSkills>().FireballCooldown;

	private void LateUpdate()
	{
		float value = (_fireballCooldown.Value - _fireballCooldown.CurrentValue) / _fireballCooldown.Value;
		_counterImage.fillAmount = value;
	}
}
