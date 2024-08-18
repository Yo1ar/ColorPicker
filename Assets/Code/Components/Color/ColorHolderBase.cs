using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.Constants;

public class ColorHolderBase : MonoBehaviour
{
	[Header("Coloring")]
	[SerializeField] protected PlayerColor Color;
	[SerializeField] protected SpriteRenderer View;
	[SerializeField] private Image _image;
	public event Action<PlayerColor> OnColorChanged;
	public PlayerColor ColorToCheck => Color;
	
	private void Awake() =>
		Recolor();

	public void SetColor(PlayerColor color)
	{
		if (name == "chase")
			Debug.Log("Set color to " + color);
		
		Color = color;
		Recolor();
	}
	
	public void Recolor()
	{
		if (View)
		{
			View.color = Colors.GetColor(Color);
			OnColorChanged?.Invoke(Color);
		}

		if (_image)
		{
			_image.color = Colors.GetColor(Color);
			OnColorChanged?.Invoke(Color);
		}
	}
}