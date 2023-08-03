using UnityEngine.Events;

namespace GameCore.Events
{
	public static class GlobalEventManager
	{
		public static readonly UnityEvent OnBackPressed = new();
		public static readonly UnityEvent OnScreenTap = new();
		public static readonly UnityEvent OnLevelLoaded = new();
		public static readonly UnityEvent OnLevelUnloaded = new();
	}
}