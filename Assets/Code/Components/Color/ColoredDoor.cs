using UnityEngine;
using Utils;

public sealed class ColoredDoor : ColorCheckerBase
{
	[Header("Door")]
	[SerializeField] private Sprite _closedSprite;
	[SerializeField] private Sprite _openedSprite;
	[SerializeField] private Collider2D _closingCollider;
	
	private void Start() =>
		SetSprite(_closedSprite);

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(@as: PlayerColorHolder))
			OpenDoor();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		CloseDoor();
	}

	private void OpenDoor()
	{
		_closingCollider.enabled = false;
		SetSprite(_openedSprite);
	}

	private void CloseDoor()
	{
		_closingCollider.enabled = true;
		SetSprite(_closedSprite);
	}

	private void SetSprite(Sprite sprite) =>
		View.sprite = sprite;
}