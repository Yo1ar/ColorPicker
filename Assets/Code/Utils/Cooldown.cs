using UnityEngine;

namespace Utils
{
	public class Cooldown
	{
		private readonly float _cdValue;
		private float _endValue;

		public bool IsReady => _endValue <= Time.time;
		public float CurrentValue => Mathf.Clamp(_endValue - Time.time, 0, _cdValue);
		public float Value => _cdValue;
		
		public Cooldown(float cdValue) =>
			_cdValue = cdValue;

		public void Reset() =>
			_endValue = Time.time + _cdValue;
	}
}