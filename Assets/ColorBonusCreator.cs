using GameCore.GameServices;
using TMPro;
using UnityEngine;
using Utils.Constants;

public sealed class ColorBonusCreator : ColorCheckerBase
{
	[Header("Color Creator")] [SerializeField]
	private WildBonus _wildBonusPrefab;

	[SerializeField] private TMP_Text _text;
	private const string NO_PROBLEM = "No problem take one more and change your color to Red";
	private const string PICK_BONUS = "Pick the Wild Color Bonus and change your color to Red";
	private const string CAN_PATH = "Now path trough the Red door";

	protected override void Awake()
	{
		base.Awake();

		if (Services.FactoryService.Player)
		{
			_text.SetText(PICK_BONUS);
			Setup();
		}
		
		Services.FactoryService.OnPlayerCreated.AddListener(Setup);
	}

	private void Setup()
	{
		CreateWildBonus();
		PlayerColorHolder.OnColorChanged += CheckNeededColor;
	}

	private void CheckNeededColor(EColors color)
	{
		if (color != EColors.Red)
		{
			_text.SetText(NO_PROBLEM);
			CreateWildBonus();
		}
		else
			_text.SetText(CAN_PATH);
	}

	private void CreateWildBonus() =>
		Instantiate(_wildBonusPrefab, transform.position, Quaternion.identity);
}