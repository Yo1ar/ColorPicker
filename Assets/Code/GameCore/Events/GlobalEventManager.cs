using System;
using Utils.Constants;

namespace GameCore.Events
{
	public static class GlobalEventManager
	{
		public static Action OnBackPressed = () => { };
		public static Action OnScreenTap = () => { };
		public static Action OnLevelLoaded = () => { };
		public static Action OnLevelUnloaded = () => { };

		public static Action OnStartPickColor = () => { };
		public static Action<PlayerColor> OnColorPicked = (color) => { };
	}
}