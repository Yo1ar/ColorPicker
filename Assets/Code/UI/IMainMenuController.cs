using UnityEngine.UIElements;

namespace UI
{
	public interface IMainMenuController : IUiController
	{
		Button SettingsButton { get; }
	}
}