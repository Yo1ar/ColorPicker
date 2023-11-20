using UnityEngine;
using Utils.Constants;

namespace Components.Color
{
	[ExecuteInEditMode]
	public sealed class ColorHolder : MonoBehaviour
	{
		[SerializeField] private EColors _color;
		[SerializeField] private SpriteRenderer _visual;
		public EColors Color => _color;

		public void Recolor() =>
			_visual.color = Colors.GetColor(_color);
	}
}