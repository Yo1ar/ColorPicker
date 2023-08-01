using System;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "GameEvents", menuName = "Events/GameEvents")]
	public class GameEvents : ScriptableObject
	{
		public Action OnBackPressed;
		public Action OnScreenTap;
	}
}