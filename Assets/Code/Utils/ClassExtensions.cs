using Components.Color;
using Components.Player;
using GameCore.GameSystems;
using UnityEngine;
using Utils.Constants;

namespace Utils
{
	public static class ClassExtensions
	{
		public static bool IsInLayer(this GameObject gameObject, LayerMask layer) =>
			layer == (layer | 1 << gameObject.layer);

		public static bool TryGetHealth(this GameObject gameObject, out PlayerHealth playerHealth) =>
			gameObject.TryGetComponent(out playerHealth);

		public static bool IsPlayer(this Collider2D collider) =>
			collider.gameObject.CompareTag(Tags.PLAYER);

		public static int SecAsMillisec(this float value) =>
			(int)(value * 1000);

		#region STRINGS

		public static bool IsEmpty(this string value) =>
			value == string.Empty;

		public static string LogItalic(this string message) =>
			$"<i>{message}</i>";

		public static string LogBold(this string message) =>
			$"<b>{message}</b>";

		public static string LogColored(this string message, string color) =>
			$"<color={color}>{message}</color>";

		#endregion

		#region VECTORS

		public static Vector2 ToVector2(this Vector3 vector3) =>
			new(vector3.x, vector3.y);

		public static Vector3 ToVector3(this Vector2 vector2) =>
			new(vector2.x, vector2.y);

		public static Vector3 AddX(this Vector3 vector3, float addedX) =>
			new(vector3.x + addedX, vector3.y);

		public static Vector3 AddY(this Vector3 vector3, float addedY) =>
			new(vector3.x, vector3.y + addedY);

		public static Vector3 Add(this Vector3 vector3, Vector3 offset) =>
			new(vector3.x + offset.x, vector3.y + offset.y, vector3.z + offset.z);

		#endregion
	}
}