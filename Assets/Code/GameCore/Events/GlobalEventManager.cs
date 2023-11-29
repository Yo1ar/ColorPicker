using UnityEngine.Events;

namespace GameCore.Events
{
	public static class GlobalEventManager
	{
		public static readonly UnityEvent OnBackPressed = new();
		public static readonly UnityEvent OnScreenTap = new();
		public static readonly UnityEvent OnLevelLoaded = new();
		public static readonly UnityEvent OnLevelUnloaded = new();

		public static readonly UnityEvent OnColorPick = new();
		public static readonly UnityEvent OnGrayColor = new();
		public static readonly UnityEvent OnRedColor = new();
		public static readonly UnityEvent OnGreenColor = new();
		public static readonly UnityEvent OnBlueColor = new();
	}
}