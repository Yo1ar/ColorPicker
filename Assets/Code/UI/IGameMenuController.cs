using UnityEngine.UIElements;

namespace UI
{
	public interface IGameMenuController : IMainMenuController
	{
		public Button ResumeButton { get; }
	}
}