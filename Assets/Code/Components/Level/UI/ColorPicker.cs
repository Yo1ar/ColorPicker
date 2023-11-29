using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using Utils.Constants;

namespace Level.UI
{
	public class ColorPicker : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private LevelUIButton _activationButton;

		[Header("Color Buttons")]
		[SerializeField] private LevelUIButton _red;
		[SerializeField] private LevelUIButton _green;
		[SerializeField] private LevelUIButton _blue;
		[SerializeField] private LevelUIButton _white;

		private readonly int _expandTriggerHash = Animator.StringToHash("expand");
		private readonly int _collapseTriggerHash = Animator.StringToHash("collapse");
		private ColorHolderBase _playerColorHolder;
		private FactoryService _factoryService;
		private IWildColorContainer _wildColorContainer;

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
			_activationButton.OnTap += ExpandColors;

			_white.OnTap += CollapseColors;
			_white.OnTap += SetPlayerGray;

			_red.OnTap += CollapseColors;
			_red.OnTap += SetPlayerRed;

			_green.OnTap += CollapseColors;
			_green.OnTap += SetPlayerGreen;

			_blue.OnTap += CollapseColors;
			_blue.OnTap += SetPlayerBlue;
		}

		private void ExpandColors()
		{
			if (!_wildColorContainer.CanUseWildColorBonus())
				return;

			_animator.Play(_expandTriggerHash);
		}

		private void CollapseColors() =>
			_animator.Play(_collapseTriggerHash);

		private void SetPlayerGray()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Gray)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Gray);
		}

		private void SetPlayerRed()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Red)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Red);
		}

		private void SetPlayerGreen()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Green)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Green);
		}

		private void SetPlayerBlue()
		{
			if (_playerColorHolder.ColorToCheck == EColors.Blue)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(EColors.Blue);
		}
	}
}