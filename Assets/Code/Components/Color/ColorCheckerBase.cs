using GameCore.GameServices;
using Utils.Constants;

public class ColorCheckerBase : ColorHolderBase
{
	protected ColorHolderBase PlayerColorHolder;

	protected virtual void Awake()
	{
		if (Services.FactoryService.Player)
			SetPlayerColorHolder();
		
		Services.FactoryService.OnPlayerCreated.AddListener(SetPlayerColorHolder);
	}

	private void SetPlayerColorHolder() =>
		PlayerColorHolder = Services.FactoryService.Player.GetComponent<ColorHolderBase>();
	
	public bool IsSameColor(ColorHolderBase @as)
	{
		if (@as.ColorToCheck == PlayerColor.White)
			return true;
		
		return @as.ColorToCheck == Color;
	}
}