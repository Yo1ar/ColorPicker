using System;
using BgTools.Utils;
using Characters.Player;
using GameCore.GameServices;
using UI;
using UnityEngine.UIElements;
using Utils;
using Utils.Constants;

namespace UI
{
	public sealed class SkillBarNew : UiElement
	{
		private SkillBarController _skillBarController;

		protected override void Awake()
		{
			base.Awake();
			_skillBarController = new SkillBarController(Document);
		}

		private void OnDisable() =>
			_skillBarController.Dispose();
	}

	public sealed class SkillBarController : IUiController
	{
		private const string k_skillbar = "skillBar";
		private const string k_jumpButton = "btn_jump";
		private const string k_fireballButton = "btn_fireball";
		private const string k_eraserButton = "btn_eraser";
		private const string k_speedButton = "btn_speed";

		private readonly Button _jumpButton;
		private readonly Button _fireballButton;
		private readonly Button _eraserButton;
		private readonly Button _speedButton;

		private readonly FactoryService _factoryService;
		private IPlayerSkills _playerSkills;
		private ColorHolderBase _playerColorHolder;

		public VisualElement RootElement { get; }

		public SkillBarController(UIDocument document)
		{
			_factoryService = Services.FactoryService;
			RootElement = document.rootVisualElement.GetVisualElement(k_skillbar);

			_jumpButton = RootElement.GetVisualElement(k_jumpButton).GetButton();
			_fireballButton = RootElement.GetVisualElement(k_fireballButton).GetButton();
			_eraserButton = RootElement.GetVisualElement(k_eraserButton).GetButton();
			_speedButton = RootElement.GetVisualElement(k_speedButton).GetButton();

			if (_factoryService.Player)
				GetPlayerColorHolder();

			_factoryService.OnPlayerCreated.AddListener(GetPlayerColorHolder);
		}

		public void Dispose() =>
			_playerColorHolder.OnColorChanged -= ChangePlayerSkill;

		private void GetPlayerColorHolder()
		{
			_playerColorHolder = _factoryService.Player.GetComponent<ColorHolderBase>();
			ChangePlayerSkill(_playerColorHolder.ColorToCheck);

			_playerColorHolder.OnColorChanged += ChangePlayerSkill;
		}

		private void ChangePlayerSkill(EColors color)
		{
			_fireballButton.visible = false;
			_eraserButton.visible = false;
			_speedButton.visible = false;

			_jumpButton.style.color = new StyleColor(Colors.GetColor(EColors.White));

			switch (color)
			{
				case EColors.Gray:
					_eraserButton.visible = true;
					break;
				case EColors.Red:
					_fireballButton.visible = true;
					break;
				case EColors.Green:
					_jumpButton.style.color = new StyleColor(Colors.GetColor(EColors.Green));
					break;
				case EColors.Blue:
					_speedButton.visible = true;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(color), color, null);
			}
		}
	}
}