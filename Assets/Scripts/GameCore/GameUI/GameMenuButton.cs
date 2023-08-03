using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameCore.GameUI
{
	public class GameMenuButton : MonoBehaviour, IPointerClickHandler
	{
		private TMP_Text _text;
		public UnityEvent OnClick;

		private void Awake() =>
			_text = GetComponent<TMP_Text>();

		public void OnPointerClick(PointerEventData eventData) =>
			OnClick?.Invoke();

		public void Underline() => 
			_text.fontStyle = FontStyles.Underline;

		public void MakeTextNormal() =>
			_text.fontStyle = FontStyles.Normal;

		public void SetText(string text) =>
			_text.SetText(text);
	}
}