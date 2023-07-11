using Components.Player;
using GameCore.GameUI;
using UnityEngine;
using Utils.Constants;

namespace Components.Level.UI
{
	public class SkillBar : MonoBehaviour
	{
		[SerializeField] private LevelUIButton _eraserBtn;
		[SerializeField] private LevelUIButton _fireballBtn;
		[SerializeField] private LevelUIButton _jumpBtn;
		[SerializeField] private JumpButtonStateSwitch _jumpBtnStateSwitch;
		
		private PlayerJump _playerJump;
		private PlayerInventory _playerInventory;
		private PlayerSkills _playerSkills;
		private GroundCheck _playerGroundCheck;

		private void Awake()
		{
			GameObject player = GameObject.FindWithTag(Tags.Player);
			_playerJump = player.GetComponent<PlayerJump>();
			_playerInventory = player.GetComponent<PlayerInventory>();
			_playerSkills = player.GetComponent<PlayerSkills>();
		}

		private void Start()
		{
			_eraserBtn.SetCounterValue(_playerInventory.erasers);
			_fireballBtn.SetCounterValue(_playerInventory.fireballs);
		}

		private void OnEnable()
		{
			_jumpBtn.OnTap += _playerJump.Jump;
			_eraserBtn.OnTap += OnEraserTap;
			_fireballBtn.OnTap += OnFireballTap;

			_playerInventory.OnEraserCountModified += _eraserBtn.SetCounterValue;
			_playerInventory.OnFireballsCountModified += _fireballBtn.SetCounterValue;
			_playerJump.CanJump += _jumpBtnStateSwitch.SetState;
		}

		private void OnDisable()
		{
			_jumpBtn.OnTap -= _playerJump.Jump;
			_eraserBtn.OnTap -= OnEraserTap;
			_fireballBtn.OnTap -= OnFireballTap;

			_playerInventory.OnEraserCountModified -= _eraserBtn.SetCounterValue;
			_playerInventory.OnFireballsCountModified -= _fireballBtn.SetCounterValue;
			_playerJump.CanJump -= _jumpBtnStateSwitch.SetState;
		}

		private void OnEraserTap() =>
			_playerSkills.ActivateErasableMode();

		private void OnFireballTap() =>
			_playerSkills.TryLaunchFireball();
	}
}