using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.GameUI
{
	public class LevelUIButton : MonoBehaviour, IPointerDownHandler
	{
		public event Action OnTap;

		public void OnPointerDown(PointerEventData eventData) =>
			OnTap?.Invoke();
	}
}