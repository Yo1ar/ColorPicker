using System;
using UnityEngine;

namespace Utils.Constants
{
	public static class Colors
	{
		public static readonly Color White = new(1f, 1f, 1f, 1f);
		public static readonly Color WhiteT = new(1f, 1f, 1f, 0.5f);
		public static readonly Color Red = new(1f, 0.2784314f, 0.2784314f, 1f);
		public static readonly Color RedT = new(1f, 0.2784314f, 0.2784314f, 0.5f);
		public static readonly Color Green = new(0.3372549f, 1, 0.2509804f, 1f);
		public static readonly Color GreenT = new(0.3372549f, 1, 0.2509804f, 0.5f);
		public static readonly Color Blue = new(0.2509804f, 0.8156863f, 1f, 1f);
		public static readonly Color BlueT = new(0.2509804f, 0.8156863f, 1f, 0.5f);
		public static Color GetColor(EColors color)
		{
			return color switch
			{
				EColors.White => White,
				EColors.Red => Red,
				EColors.Green => Green,
				EColors.Blue => Blue,
				_ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
			};
		}
	}

	public enum EColors
	{
		White = 0,
		Red = 1,
		Green = 2,
		Blue = 3,
	}
}