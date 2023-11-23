using Characters.Player;
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
		private PlayerController _playerController;

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
			_playerController = _factoryService.Player.GetComponent<PlayerController>();
		}

		private void OnEnable()
		{
			_activationButton.OnTap += ExpandColors;

			_white.OnTap += CollapseColors;
			_white.OnTap += SetPlayerWhite;

			_red.OnTap += CollapseColors;
			_red.OnTap += SetPlayerRed;

			_green.OnTap += CollapseColors;
			_green.OnTap += SetPlayerGreen;

			_blue.OnTap += CollapseColors;
			_blue.OnTap += SetPlayerBlue;
		}

		private void ExpandColors()
		{
			if (!_playerController.TrySpendWildColorBonus())
				return;

			_animator.Play(_expandTriggerHash);
		}

		private void CollapseColors() =>
			_animator.Play(_collapseTriggerHash);

		private void SetPlayerWhite()
		{
			_playerColorHolder.SetColor(EColors.White);
		}

		private void SetPlayerRed() =>
			_playerColorHolder.SetColor(EColors.Red);

		private void SetPlayerGreen() =>
			_playerColorHolder.SetColor(EColors.Green);

		private void SetPlayerBlue() =>
			_playerColorHolder.SetColor(EColors.Blue);
	}
}