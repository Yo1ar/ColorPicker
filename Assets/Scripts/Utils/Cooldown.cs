using UnityEngine;

namespace Utils
{
	public class Cooldown
	{
		private readonly float _cdValue;
		private float _endValue;

		public bool isReady => _endValue <= Time.time;

		public Cooldown(float cdValue) =>
			_cdValue = cdValue;

		public void Reset() =>
			_endValue = Time.time + _cdValue;
	}
}