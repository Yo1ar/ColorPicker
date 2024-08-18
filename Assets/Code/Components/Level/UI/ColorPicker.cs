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
		private readonly int _collapseTriggerHash = Animator.StringToHash("collapse");

		private readonly int _expandTriggerHash = Animator.StringToHash("expand");
		private bool _canPickColor;
		private ColorHolderBase _playerColorHolder;
		private FactoryService _factoryService;
		private IWildColorContainer _wildColorContainer;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_factoryService = Services.FactoryService;

			if (_factoryService.Player)
				ConnectPlayerColorHolder();
		}

		private void Start()
		{
			SetHintsActive(false);
		}

		private void OnEnable()
		{
			_factoryService.OnPlayerCreated.AddListener(ConnectPlayerColorHolder);
			GlobalEventManager.OnStartPickColor += ExpandColors;
			GlobalEventManager.OnColorPicked += CollapseColors;
			GlobalEventManager.OnColorPicked += SetPlayerColor;
		}

		private void OnDisable()
		{
			_factoryService.OnPlayerCreated.RemoveListener(ConnectPlayerColorHolder);
			GlobalEventManager.OnStartPickColor -= ExpandColors;
			GlobalEventManager.OnColorPicked -= CollapseColors;
			GlobalEventManager.OnColorPicked -= SetPlayerColor;
		}

		private void SetPlayerColor(PlayerColor color)
		{
			if (_playerColorHolder.ColorToCheck == color)
				return;

			if (_wildColorContainer.TryUseWildColorBonus())
				_playerColorHolder.SetColor(color);

			_canPickColor = false;
		}

		private void ConnectPlayerColorHolder()
		{
			_playerColorHolder = _factoryService.Player.GetComponent<ColorHolderBase>();
			_wildColorContainer = _factoryService.Player.GetComponent<IWildColorContainer>();
		}

		private void ExpandColors()
		{
			if (_canPickColor)
			{
				CollapseColors(PlayerColor.White);
				_canPickColor = false;
				return;
			}

			if (!_wildColorContainer.CanUseWildColorBonus())
				return;

			_canPickColor = true;

			_animator.Play(_expandTriggerHash);
			SetHintsActive(true);
		}

		private void CollapseColors(PlayerColor _)
		{
			_animator.Play(_collapseTriggerHash);
			SetHintsActive(false);
		}

		private void SetHintsActive(bool value)
		{
			foreach (CanvasGroup hint in _hints)
				hint.alpha = value ? 1 : 0;
		}
	}
}