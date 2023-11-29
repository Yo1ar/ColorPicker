using UnityEngine;
using Utils;

[SelectionBase]
public sealed class ColoredPlatform : ColorCheckerBase
{
	[Header("Platform")] [SerializeField] private BoxCollider2D _platformCollider;

	protected override void Awake()
	{
		base.Awake();
		PlatformTurnOn();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(PlayerColorHolder))
			PlatformTurnOn();
		else
			PlatformTurnOff();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(PlayerColorHolder))
			PlatformTurnOn();
		else
			PlatformTurnOff();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		PlatformTurnOff();
	}

	private void PlatformTurnOn() =>
		_platformCollider.enabled = true;

	private void PlatformTurnOff() =>
		_platformCollider.enabled = false;
}