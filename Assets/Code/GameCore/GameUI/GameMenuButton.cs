using GameCore.GameServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameCore.GameUI
{
	public class GameMenuButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
	{
		private TMP_Text _text;
		public UnityEvent OnClick;
		private AudioClip _clickClip;
		private AudioSourcesController _audioController;
		
		private void Awake()
		{
			_audioController = Services.AudioService.AudioSourcesController;
			_clickClip = Services.AssetService.SoundsConfig.ClickClip;
			_text = GetComponent<TMP_Text>();
		}

		public void OnPointerDown(PointerEventData eventData) =>
			_audioController.PlaySoundOneShot(_clickClip);

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