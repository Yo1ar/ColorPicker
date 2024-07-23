using UnityEngine;

namespace UI.CustomElements
{
	public static class TextureFillElementExtensions
	{
		public static float GetAngle(this FillStart fillStart) =>
			fillStart switch
			{
				FillStart.Up => -90f,
				FillStart.Left => -180f,
				FillStart.Down => -270f,
				FillStart.Right => 0,
				_ => 0,
			};
		
		public static float ToRadians(this float degrees) => degrees * Mathf.Deg2Rad;
	}
}