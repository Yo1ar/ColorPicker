using GameCore.GameServices;
using TMPro;
using UnityEngine;
using Utils.Constants;

public sealed class LiveHint : ColorCheckerBase
{
	[Header("Hint")]
	[SerializeField] private ColorCheckerBase _hintObject;
	[SerializeField] private RectTransform _view;
	[SerializeField] private TMP_Text _text;
	
	private const string WHITE = "White color";
	private const string SAME = "Same as you";
	private FactoryService _factoryService;

	protected override void Awake()
	{
		base.Awake();

		if (_hintObject.ColorToCheck == EColors.White)
		{
			SetColor(EColors.White);
			_text.SetText(WHITE);
			return;
		}
		else
			_text.SetText(SAME);

		_factoryService = Services.FactoryService;

		if (_factoryService.Player)
			ConnectToPlayerChangeColor();

		_factoryService.OnPlayerCreated.AddListener(ConnectToPlayerChangeColor);
	}

	private void ConnectToPlayerChangeColor()
	{
		PlayerColorCheck(PlayerColorHolder.ColorToCheck);
		PlayerColorHolder.OnColorChanged += PlayerColorCheck;
	}

	private void PlayerColorCheck(EColors color)
	{
		if (_hintObject.IsSameColor(@as: PlayerColorHolder))
		{
			SetColor(PlayerColorHolder.ColorToCheck);
			_view.gameObject.SetActive(true);
		}
		else
			_view.gameObject.SetActive(false);
	}
}