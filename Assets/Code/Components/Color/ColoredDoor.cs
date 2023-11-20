using GameCore.GameServices;
using UnityEngine;
using Utils;

namespace Components.Color
{
	public class ColoredDoor : MonoBehaviour
	{
		[SerializeField] private Sprite _closedSprite;
		[SerializeField] private Sprite _openedSprite;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private ColorChecker _colorChecker;
		private ColorHolder _colorHolder;

		private void Awake()
		{
			if (Services.FactoryService.Player)
				SetPlayerColorHolder();
			else
				Services.FactoryService.OnPlayerCreated.AddListener(SetPlayerColorHolder);
		}

		private void Start() =>
			SetSprite(_closedSprite);

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			_colorHolder = other.GetComponent<ColorHolder>();

			if (_colorChecker.IsSameColor(@as: _colorHolder))
				SetSprite(_openedSprite);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			if (_colorChecker.IsSameColor(@as: _colorHolder))
				SetSprite(_closedSprite);
		}

		private void SetSprite(Sprite sprite) =>
			_spriteRenderer.sprite = sprite;

		private void SetPlayerColorHolder() =>
			_colorHolder = Services.FactoryService.Player.GetComponent<ColorHolder>();
	}
}