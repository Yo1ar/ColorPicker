using GameCore.GameServices;
using UnityEngine;
using Utils;

public sealed class ColoredDoor : ColorCheckerBase
{
	[Header("Door")] [SerializeField] private Sprite _closedSprite;
	[SerializeField] private Sprite _openedSprite;
	[SerializeField] private Collider2D _closingCollider;

	private bool _isOpened;
	private AudioSourcesController _audioSourcesController;
	private AudioClip _openSound;
	private AudioClip _closeSound;

	protected override void Awake()
	{
		base.Awake();
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		_openSound = Services.AssetService.SoundsConfig.DoorOpenClip;
		_closeSound = Services.AssetService.SoundsConfig.DoorCloseClip;
	}

	private void Start() =>
		SetSprite(_closedSprite);

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(@as: PlayerColorHolder) && !_isOpened)
			OpenDoor();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.IsPlayer() || !_isOpened)
			return;

		CloseDoor();
	}

	private void OpenDoor()
	{
		_audioSourcesController.PlaySoundOneShot(_openSound);
		_isOpened = true;
		_closingCollider.enabled = false;
		SetSprite(_openedSprite);
	}

	private void CloseDoor()
	{
		_audioSourcesController.PlaySoundOneShot(_closeSound);
		_isOpened = false;
		_closingCollider.enabled = true;
		SetSprite(_closedSprite);
	}

	private void SetSprite(Sprite sprite) =>
		View.sprite = sprite;
}