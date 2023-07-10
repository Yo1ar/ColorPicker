using System;
using Components.Level.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.GameUI
{
	public class LevelUIButton : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private LevelUIButtonCounter _buttonCounter;
		private Image _buttonImage;
		public event Action OnTap;

		private void Awake() => 
			_buttonImage = GetComponent<Image>();

		public void OnPointerDown(PointerEventData eventData) => 
			OnTap?.Invoke();

		public void SetCounterValue(int value)
		{
			if (value < 1)
			{
				_buttonCounter.TurnOff();
				_buttonImage.enabled = false;
			}
			else
			{
				_buttonCounter.TurnOn();
				_buttonImage.enabled = true;
			}

			_buttonCounter.SetValue(value);
		}
	}
}