using System;
using UnityEngine;

namespace Utils.Constants
{
	public static class Colors
	{
		public static readonly Color White = new(1f, 1f, 1f, 1f);
		public static readonly Color Gray = new(0.6f, 0.6f, 0.6f, 1f);
		public static readonly Color GrayT = new(0.5f, 0.5f, 0.5f, 0.5f);
		public static readonly Color Red = new(1f, 0.2784314f, 0.2784314f, 1f);
		public static readonly Color RedT = new(1f, 0.2784314f, 0.2784314f, 0.5f);
		public static readonly Color Green = new(0.3372549f, 1f, 0.2509804f, 1f);
		public static readonly Color GreenT = new(0.3372549f, 1f, 0.2509804f, 0.5f);
		public static readonly Color Blue = new(0.2509804f, 0.8156863f, 1f, 1f);
		public static readonly Color BlueT = new(0.2509804f, 0.8156863f, 1f, 0.5f);
		public static readonly Color Transparent = new(0, 0, 0, 0);

		public static Color GetColor(PlayerColor color) =>
			color switch
			{
				PlayerColor.White => White,
				PlayerColor.Red => Red,
				PlayerColor.Green => Green,
				PlayerColor.Blue => Blue,
				PlayerColor.Gray => Gray,
				_ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
			};

		public static string GetLogColor(PlayerColor color) =>
			color switch
			{
				PlayerColor.White => "white",
				PlayerColor.Red => "red",
				PlayerColor.Green => "green",
				PlayerColor.Blue => "blue",
				PlayerColor.Gray => "gray",
				_ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
			};
	}

	public enum PlayerColor
	{
		White = 0,
		Red = 1,
		Green = 2,
		Blue = 3,
		Gray = 4,
	}
}