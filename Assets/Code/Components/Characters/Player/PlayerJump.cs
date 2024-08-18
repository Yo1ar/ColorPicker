using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;
using Utils.Constants;

namespace Characters.Player
{
	[RequireComponent(typeof(GroundCheck), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
	public class PlayerJump : MonoBehaviour
	{
		[SerializeField] private float _jumpForce;
		
		private Rigidbody2D _rigidbody2D;
		private GroundCheck _groundCheck;
		private ColorHolderBase _colorHolder;
		private bool _canDoubleJump;
		private AudioClip _jumpSound;
		private AudioClip _jumpSound2;
		private AudioSourcesController _audioController;

		private void Awake()
		{
			_audioController = Services.AudioService.AudioSourcesController;
			_jumpSound = Services.AssetService.SoundsConfig.JumpClip;
			_jumpSound2 = Services.AssetService.SoundsConfig.JumpClip2;
			_colorHolder = GetComponent<ColorHolderBase>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_groundCheck = GetComponent<GroundCheck>();
		}

		private void OnEnable() =>
			PlayerEventManager.OnJump.AddListener(Jump);

		private void Jump()
		{
			if (CanJump())
			{
				_audioController.PlaySoundOneShot(_jumpSound);
				_canDoubleJump = true;
				AddJumpForce();
				return;
			}

			if (CanDoubleJump())
			{
				_audioController.PlaySoundOneShot(_jumpSound2);
				_canDoubleJump = false;
				AddJumpForce();
			}
		}

		private void AddJumpForce()
		{
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
		}

		private bool CanJump() =>
			_groundCheck.IsGrounded;

		private bool CanDoubleJump()
		{
			if (_colorHolder.ColorToCheck != PlayerColor.Green)
				return false;

			if (!_canDoubleJump)
				return false;
			
			return true;
		}
	}
}