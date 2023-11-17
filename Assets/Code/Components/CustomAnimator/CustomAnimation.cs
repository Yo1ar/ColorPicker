using System;
using UnityEngine;

namespace Components.CustomAnimator
{
	[Serializable]
	public class CustomAnimation
	{
		[SerializeField] private string _name;
		[SerializeField] private bool _randomFrameStart;
		[SerializeField] private bool _loop;
		[SerializeField] private Sprite[] _frames;

		public string name => _name;
		public bool randomFrameStart => _randomFrameStart;
		public bool loop => _loop;
		public Sprite[] frames => _frames;
	}
}