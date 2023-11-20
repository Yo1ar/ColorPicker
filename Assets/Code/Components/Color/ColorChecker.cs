using GameCore.GameSystems;
using UnityEngine;
using Utils.Constants;

namespace Components.Color
{
	public sealed class ColorChecker : MonoBehaviour
	{
		[SerializeField] private EColors _colorToCheck;
		private ColoringSystem _coloringSystem;
		public EColors ColorToCheck => _colorToCheck;

		private void Awake() =>
			_coloringSystem = Systems.ColoringSystem;

		private void CheckColor()
		{
			
		}

		private void OnTriggerEnter(Collider other)
		{
			other.TryGetComponent(out ColorHolder holder);
			// ColoringSystem
		}
	}
}