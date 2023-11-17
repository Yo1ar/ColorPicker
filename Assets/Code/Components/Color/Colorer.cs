using UnityEngine;
using Utils.Constants;

namespace Components.Color
{
	public class Colorer : MonoBehaviour
	{
		[SerializeField] protected EColors _color;
		[SerializeField] private bool _recolorThis;
		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			if (!_recolorThis)
				return;
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			SetObjectColor(_color);
		}

		public void SetObjectColor(EColors color)
		{
			_spriteRenderer.color = Colors.GetColor(color);
			_color = color;
		}
	}
}