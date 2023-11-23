using GameCore.Events;
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
		private IWildColorContainer _wildColorContainer;
		private ColorHolderBase _colorHolder;
		private bool _canDoubleJump;

		private void Awake()
		{
			_colorHolder = GetComponent<ColorHolderBase>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_groundCheck = GetComponent<GroundCheck>();
			_wildColorContainer = GetComponent<IWildColorContainer>();
		}

		private void OnEnable() =>
			PlayerEventManager.OnJump.AddListener(Jump);

		private void Jump()
		{
			if (CanJump())
			{
				_canDoubleJump = true;
				AddJumpForce();
				return;
			}

			if (CanDoubleJump())
			{
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
			if (_colorHolder.ColorToCheck != EColors.Green)
				return false;

			if (_wildColorContainer.TrySpendWildColorBonus())
				return false;

			if (!_canDoubleJump)
				return false;
			
			_canDoubleJump = false;
			return true;
		}
	}
}