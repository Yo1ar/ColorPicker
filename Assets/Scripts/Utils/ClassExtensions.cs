using UnityEngine;
using Utils.Constants;

namespace Utils
{
	public static class ClassExtensions
	{
		public static bool IsInLayer(this GameObject gameObject, LayerMask layer) =>
			layer == (layer | 1 << gameObject.layer);

		public static bool IsPlayer(this Collider2D collider) =>
			collider.gameObject.CompareTag(Tags.Player);

		public static int AsMilliseconds(this float value) =>
			(int)(value * 1000);

		#region Vectors

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