using System;
using Components.Characters.Player;
using Components.Player;
using GameCore.Events;
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
		private PlayerJump _playerJump;
		private PlayerInventory _playerInventory;
		private PlayerSkillsOld _playerSkillsOld;
		private GroundCheck _playerGroundCheck;

		private void Awake()
		{
			_factoryService = Services.FactoryService;

			if (_factoryService.Player != null)
				InitPLayer();
			else
				_factoryService.OnPlayerCreated.AddListener(InitPLayer);
		}

		private void OnEnable()
		{
			PlayerEventManager.OnErase.AddListener(OnEraserTap);
			PlayerEventManager.OnShoot.AddListener(OnFireballTap);
		}

		private void OnDisable()
		{
			_factoryService.OnPlayerCreated.RemoveListener(InitPLayer);
			PlayerEventManager.OnErase.RemoveListener(OnEraserTap);
			PlayerEventManager.OnShoot.RemoveListener(OnFireballTap);
		}

		private void InitPLayer()
		{
			GetPlayerComponents();
			SubscribeToPlayerActions();
		}

		private void GetPlayerComponents()
		{
			_playerInventory = _factoryService.Player.GetComponent<PlayerInventory>();
			_playerSkillsOld = _factoryService.Player.GetComponent<PlayerSkillsOld>();
			_playerJump = _factoryService.Player.GetComponent<PlayerJump>();
		}

		private void SubscribeToPlayerActions()
		{
			_eraserBtn.SetCounterValue(_playerInventory.Erasers);
			_fireballBtn.SetCounterValue(_playerInventory.Fireballs);

			_playerInventory.OnEraserCountModified.AddListener(_eraserBtn.SetCounterValue);
			_playerInventory.OnFireballsCountModified.AddListener(_fireballBtn.SetCounterValue);
			_playerJump.CanJump.AddListener(_jumpBtnStateSwitch.SetState);
		}

		private void OnEraserTap() =>
			_playerSkillsOld.SwitchErasableMode();

		private void OnFireballTap() =>
			_playerSkillsOld.TryLaunchFireball();
	}
}