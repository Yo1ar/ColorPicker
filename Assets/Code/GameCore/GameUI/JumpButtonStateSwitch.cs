using Components.Characters.Player;
using Components.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.GameUI
{
	public class JumpButtonStateSwitch : MonoBehaviour
	{
		[SerializeField] private Image _inactiveImage;
		[SerializeField] private Image _activeImage;
		private Rigidbody2D _rigidbody;
		private PlayerJump _playerJump;

		private void Awake() =>
			SetState(true);

		public void SetState(bool value)
		{
			_activeImage.enabled = value;
			_inactiveImage.enabled = !value;
		}
	}
}