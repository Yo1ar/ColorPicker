using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Constants;

namespace Utils
{
	public static class ClassExtensions
	{
		public static bool IsPlayer(this Collider2D collider) =>
			collider.gameObject.CompareTag(Tags.PLAYER);

		public static Vector3 AddY(this Vector3 vector3, float addedY) =>
			new(vector3.x, vector3.y + addedY);

		public static IEnumerable<Transform> Children(this Transform transform)
		{
			foreach (Transform child in transform)
				yield return child;
		}

		public static Transform GetChild(this Transform transform, int index)
		{
			int i = 0;

			foreach (Transform child in transform.Children())
			{
				if (i == index)
					return child;

				i++;
			}

			return null;
		}

		public static VisualElement GetVisualElement(this VisualElement visualElement, string name) =>
			visualElement.Q<VisualElement>(name);

		public static Button GetButton(this VisualElement visualElement, string name = "btn") =>
			visualElement.Q<Button>(name);

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

#region Tween

		public static void DoPop(this Transform transform, float scaleValue = 1.3f, float time1 = 0.1f, float time2 = 0.3f, int repeatTimes = 1)
		{
			Vector3 oldScale = transform.localScale;
			var newScale = new Vector3(scaleValue, scaleValue, scaleValue);
			Sequence sequence = DOTween.Sequence()
				.SetLoops(repeatTimes)
				.Append(SetNewScale())
				.Append(SetOldScale())
				.Play();

			Tween SetNewScale() =>
				transform.DOScale(newScale, time1);

			Tween SetOldScale() =>
				transform.DOScale(oldScale, time2);
		}

#endregion
	}
}