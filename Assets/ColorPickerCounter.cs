using GameCore.GameServices;
using TMPro;
using UnityEngine;
using Utils;

public sealed class ColorPickerCounter : MonoBehaviour
{
	[SerializeField] private TMP_Text _text;
	[SerializeField] private CanvasGroup _canvasGroup;
	private FactoryService _factoryService;
	private IWildColorContainer _colorContainer;

	private void Awake()
	{
		_factoryService = Services.FactoryService;
		
		if (_factoryService.Player)
			ConnectPlayer();
		
		_factoryService.OnPlayerCreated.AddListener(ConnectPlayer);
	}

	private void ConnectPlayer()
	{
		_colorContainer = _factoryService.Player.GetComponent<IWildColorContainer>();
		_colorContainer.OnCountChange += RefreshCount;
		RefreshCount(_colorContainer.WildColorBonusCount);
	}

	private void RefreshCount(int newCount)
	{
		_text.SetText(newCount.ToString());
		transform.DoPop();
		_canvasGroup.transform.GetChild(1).DoPop(scaleValue: 2);
		_canvasGroup.interactable = newCount > 0;
		_canvasGroup.blocksRaycasts = newCount > 0;
	}
}