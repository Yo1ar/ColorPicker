using Components.Color;
using Components.CustomAnimator;
using UnityEngine;
using Utils;

namespace Components.Level
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D _blockerCollider;
		private SpriteStateAnimation _stateAnimation;
		private ColorCheck _colorCheck;

		private void Awake()
		{
			_stateAnimation = GetComponentInChildren<SpriteStateAnimation>();
			_colorCheck = GetComponent<ColorCheck>();
		}

		private void Start() =>
			CloseDoor();

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.IsPlayer() && ColorMatch(other))
				OpenDoor();
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.IsPlayer() && ColorMatch(other))
				CloseDoor();
		}

		private bool ColorMatch(Collider2D other)
		{
			var colorHolder = other.GetComponent<ColorHolder>();
			return _colorCheck.Check(colorHolder.color);
		}

		private void OpenDoor()
		{
			_stateAnimation.SetState("open");
			_blockerCollider.enabled = false;
		}

		private void CloseDoor()
		{
			_stateAnimation.SetState("close");
			_blockerCollider.enabled = true;
		}
	}
}