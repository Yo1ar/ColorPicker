using System;
using Characters.Player;
using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using Utils.Constants;

namespace Level.UI
{
	public class SkillBar : MonoBehaviour
	{
		[SerializeField] private LevelUIButton _jumpButton;
		[SerializeField] private LevelUIButton _fireballButton;
		[SerializeField] private LevelUIButton _eraserButton;
		[SerializeField] private LevelUIButton _speedUpButton;

		private FactoryService _factoryService;
		private IWildColorContainer _wildColorContainer;
		private IPlayerSkills _playerSkills;
		private ColorHolderBase _playerColorHolder;

		private void Awake()
		{
			_factoryService = Services.FactoryService;

			if (_factoryService.Player)
				ConnectEvents();
			
			Services.FactoryService.OnPlayerCreated.AddListener(ConnectEvents);
		}

		private void ConnectEvents()
		{
			_wildColorContainer = Services.FactoryService.Player.GetComponent<IWildColorContainer>();
			_playerColorHolder = Services.FactoryService.Player.GetComponent<ColorHolderBase>();
			ChangePlayerSkill(_playerColorHolder.ColorToCheck);
			
			_playerColorHolder.OnColorChanged += ChangePlayerSkill;
		}

		private void ChangePlayerSkill(EColors color)
		{
			_fireballButton.gameObject.SetActive(false);
			_eraserButton.gameObject.SetActive(false);
			_speedUpButton.gameObject.SetActive(false);
			
			if (_jumpButton.TryGetComponent(out ColorHolderBase holder))
				holder.SetColor(EColors.White);

			switch (color)
			{
				case EColors.White:
					_eraserButton.gameObject.SetActive(true);
					break;
				case EColors.Red:
					_fireballButton.gameObject.SetActive(true);
					break;
				case EColors.Green:
					holder.SetColor(EColors.Green);
					break;
				case EColors.Blue:
					_speedUpButton.gameObject.SetActive(true);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(color), color, null);
			}
		}
	}
}