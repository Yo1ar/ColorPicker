using UnityEngine;
using Utils.Constants;

namespace Components.Color
{
	public sealed class ColorChecker : MonoBehaviour
	{
		[SerializeField] private EColors _colorToCheck;
		public EColors ColorToCheck => _colorToCheck;
		
		public bool IsSameColor(ColorHolder @as) =>
			@as.Color == _colorToCheck;
	}
}