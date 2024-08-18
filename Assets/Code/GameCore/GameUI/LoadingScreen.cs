using GameCore.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.GameUI
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;

		private const string TextLoading = "Loading...";
		private const string TextTapOnScreen = "Tap to Play";
		private ShowHideCanvasGroup _showHideCanvas;

		public UnityEvent OnShown => _showHideCanvas.OnShown;
		private UnityEvent OnHided => _showHideCanvas.OnHided;

		private void Awake() =>
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();

		private void Start() =>
			Show();

		private void OnEnable() =>
			OnHided.AddListener(SetLoadingText);

		private void OnDisable() =>
			OnHided.RemoveListener(SetLoadingText);

		public void Show() =>
			_showHideCanvas.Show();

		public void Hide()
		{
			GlobalEventManager.OnScreenTap -= Hide;
			_showHideCanvas.Hide();
		}

		public void MakeReadyToPlay() =>
			_text.SetText(TextTapOnScreen);

		private void SetLoadingText() =>
			_text.SetText(TextLoading);
	}
}