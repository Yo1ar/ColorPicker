using Components.Color;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Components.Level
{
	public class Marker : Colorer
	{
		[SerializeField] private SpriteRenderer _coloredRenderer1;
		[SerializeField] private SpriteRenderer _coloredRenderer2;

		private void Start()
		{
			_coloredRenderer1.color = Colors.GetColor(_color);
			_coloredRenderer2.color = Colors.GetColor(_color);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.IsPlayer() && HasColorHolder(other, out ColorHolder colorHolder))
				colorHolder.SetObjectColor(_color);
		}

		private static bool HasColorHolder(Collider2D collider, out ColorHolder recolor) =>
			collider.TryGetComponent(out recolor);
	}
}