using UnityEngine.Events;

namespace GameCore.Events
{
	public static class PlayerEventManager
	{
		public static UnityEvent<float> OnMove { get; } = new();
		public static UnityEvent OnJump { get; } = new();
		public static UnityEvent OnShoot { get; } = new();
		public static UnityEvent OnErase { get; } = new();
		public static UnityEvent OnSpeedUp { get; } = new();
	}
}