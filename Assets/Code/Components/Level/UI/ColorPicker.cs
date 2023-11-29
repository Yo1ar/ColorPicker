using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;
using Utils.Constants;

namespace Level.UI
{
	public class ColorPicker : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private CanvasGroup[] _hints;

		private readonly int _expandTriggerHash = Animator.StringToHash("expand");
		private readonly int _collapseTriggerHash = Animator.StringToHash("collapse");
		private ColorHolderBase _playerColorHolder;
		private FactoryService _factoryService;
		private IWildColorContainer _wildColorContainer;
		private bool _canPickColor;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_factoryService = Services.FactoryService;

			if (_factoryService.Player)
				ConnectPlayerColorHolder();

			_factoryService.OnPlayerCreated.AddListener(ConnectPlayerColorHolder);
		}

		private void ConnectPlayerColorHolder()
		{
			_playerColorHolder = _factoryService.Player.GetComponent<ColorHolderBase>();
			_wildColorContainer = _factoryService.Player.GetComponent<IWildColorContainer>();
		}

		private void OnEnable()
		{
			GlobalEventManager.OnColorPick.AddListener(ExpandColors);

			GlobalEventManager.OnGrayColor.AddListener(CollapseColors);
			GlobalEventManager.OnGrayColor.AddListener(SetPlayerGray);

			GlobalEventManager.OnRedColor.AddListener(CollapseColors);
			GlobalEventManager.OnRedColor.AddListener(SetPlayerRed);

			GlobalEventManager.OnGreenColor.AddListener(CollapseColors);
			GlobalEventManager.OnGreenColor.AddListener(SetPlayerGreen);

			GlobalEventManager.OnBlueColor.AddListener(CollapseColors);
			GlobalEventManager.OnBlueColor.AddListener(SetPlayerBlue);
		}

		private void ExpandColors()
		{
			if (_canPickColor)
			{
				CollapseColors();
				_canPickColor = false;
				return;
			}

			if (!_wildColorContainer.CanUseWildColorBonus())
				return;

			_canPickColor = true;

			_animator.Play(_expandTriggerHash);
			SetHintsActive(true);
		}

		private void CollapseColors()
		{
			_animator.Play(_collapseTriggerHash);
			SetHintsActive(false);
		}

		private void SetPlayerGray()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Gray)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Gray);

			_canPickColor = false;
		}

		private void SetPlayerRed()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Red)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Red);

			_canPickColor = false;
		}

		private void SetPlayerGreen()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Green)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Green);

			_canPickColor = false;
		}

		private void SetPlayerBlue()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Blue)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Blue);

			_canPickColor = false;
		}

		private void SetHintsActive(bool value)
		{
			foreach (CanvasGroup hint in _hints)
				hint.alpha = value ? 1 : 0;
		}
	}
}