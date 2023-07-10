namespace Components.Level.Draw
{
	using System;
	using UnityEngine;
	
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class Drawable : MonoBehaviour
	{
		private const float InvisibleAlpha = 0.001f;
		private const float VisibleAlpha = 1f;
		private bool _isDrawn;
		public event Action<Vector3, Drawable> OnVisible;
		private SpriteRenderer _spriteRenderer;

		private SpriteRenderer SpriteRenderer =>
			_spriteRenderer ??= GetComponent<SpriteRenderer>();

		private void OnBecameVisible()
		{
			if (_isDrawn)
				return;

			OnVisible?.Invoke(transform.position, this);
			_isDrawn = true;
		}

		public void SetInvisible() =>
			ModifyRendererColorAlpha(InvisibleAlpha);

		public void SetVisible() =>
			ModifyRendererColorAlpha(VisibleAlpha);

		private void ModifyRendererColorAlpha(float newAlpha) =>
			SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, newAlpha);
	}
}