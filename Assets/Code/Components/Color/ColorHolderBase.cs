using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.Constants;

public class ColorHolderBase : MonoBehaviour
{
	[Header("Coloring")]
	[SerializeField] protected EColors Color;
	[SerializeField] protected SpriteRenderer View;
	[SerializeField] private Image _image;
	public event Action<EColors> OnColorChanged;
	public EColors ColorToCheck => Color;
	
	private void Awake() =>
		Recolor();

	public void SetColor(EColors color)
	{
		if (name == "chase")
			Debug.Log("Set color to " + color.ToString());
		
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