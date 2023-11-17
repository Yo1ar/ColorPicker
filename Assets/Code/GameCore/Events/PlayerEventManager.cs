using UnityEngine.Events;

namespace GameCore.Events
{
	public static class PlayerEventManager
	{
		public static readonly UnityEvent<float> OnMove = new();
		public static readonly UnityEvent OnJump = new();
		public static readonly UnityEvent OnShoot = new();
		public static readonly UnityEvent OnErase = new();
	}
}