using UnityEngine;
using Utils.Constants;

namespace Components.Color
{
	[ExecuteInEditMode]
	public sealed class ColorHolder : MonoBehaviour
	{
		[SerializeField] private EColors _color;
		[SerializeField] private SpriteRenderer _view;
		public EColors Color => _color;

		private void Awake() =>
			Recolor();

		public void Recolor() =>
			_view.color = Colors.GetColor(_color);
	}
}