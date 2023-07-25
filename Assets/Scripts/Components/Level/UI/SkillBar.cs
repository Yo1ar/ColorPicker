using Components.Player;
using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;

namespace Components.Level.UI
{
	public class SkillBar : MonoBehaviour
	{
		[SerializeField] private LevelUIButton _eraserBtn;
		[SerializeField] private LevelUIButton _fireballBtn;
		[SerializeField] private JumpButtonStateSwitch _jumpBtnStateSwitch;

		private FactoryService _factoryService;
		private InputService _inputService;
		private PlayerJump _playerJump;
		private PlayerInventory _playerInventory;
		private PlayerSkills _playerSkills;
		private GroundCheck _playerGroundCheck;

		private void Awake()
		{
			_factoryService = Services.FactoryService;
			_inputService = Services.InputService;
			_playerInventory = _factoryService.Player.GetComponent<PlayerInventory>();
			_playerSkills = _factoryService.Player.GetComponent<PlayerSkills>();
			_playerJump = _factoryService.Player.GetComponent<PlayerJump>();
		}

		private void Start()
		{
			_eraserBtn.SetCounterValue(_playerInventory.erasers);
			_fireballBtn.SetCounterValue(_playerInventory.fireballs);
		}

		private void OnEnable()
		{
			_inputService.OnErasePressed += OnEraserTap;
			_inputService.OnLaunchFireballPressed += OnFireballTap;

			_playerInventory.OnEraserCountModified += _eraserBtn.SetCounterValue;
			_playerInventory.OnFireballsCountModified += _fireballBtn.SetCounterValue;
			_playerJump.CanJump += _jumpBtnStateSwitch.SetState;
		}

		private void OnDisable()
		{
			_inputService.OnErasePressed -= OnEraserTap;
			_inputService.OnLaunchFireballPressed -= OnFireballTap;

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