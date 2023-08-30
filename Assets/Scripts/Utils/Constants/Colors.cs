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
		public static readonly Color Green = new(0.3372549f, 1f, 0.2509804f, 1f);
		public static readonly Color GreenT = new(0.3372549f, 1f, 0.2509804f, 0.5f);
		public static readonly Color Blue = new(0.2509804f, 0.8156863f, 1f, 1f);
		public static readonly Color BlueT = new(0.2509804f, 0.8156863f, 1f, 0.5f);

		public static Color GetColor(EColors color) =>
			color switch
			{
				EColors.White => White,
				EColors.Red => Red,
				EColors.Green => Green,
				EColors.Blue => Blue,
				EColors.Yellow => Color.yellow,
				EColors.Cyan => Color.cyan,
				_ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
			};

		public static string GetLogColor(EColors color) =>
			color switch
			{
				EColors.White => "white",
				EColors.Red => "red",
				EColors.Green => "green",
				EColors.Blue => "blue",
				EColors.Yellow => "yellow",
				EColors.Cyan => "cyan",
				_ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
			};
	}

	public enum EColors
	{
		White = 0,
		Red = 1,
		Green = 2,
		Blue = 3,
		Yellow = 4,
		Cyan = 5,
	}
}