using GameCore.GameServices;

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
	
	public bool IsSameColor(ColorHolderBase @as) =>
		@as.ColorToCheck == Color;
}