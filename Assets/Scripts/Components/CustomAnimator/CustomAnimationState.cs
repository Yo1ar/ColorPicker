using System;
using UnityEngine;

namespace Components.CustomAnimator
{
	[Serializable]
	public sealed class CustomAnimationState
	{
		[SerializeField] private string _name;
		[SerializeField] private Sprite _sprite;

		public string name => _name;
		public Sprite sprite => _sprite;
	}
}