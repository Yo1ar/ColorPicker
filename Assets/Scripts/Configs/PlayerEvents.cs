using System;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "PlayerEvents", menuName = "Events/PlayerEvents")]
	public class PlayerEvents : ScriptableObject
	{
		public Action<float> OnMove;
		public Action OnJump;
		public Action OnShoot;
		public Action OnErase;
	}
}