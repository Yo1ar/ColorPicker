using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.GameUI
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;
		private ShowHideCanvasGroup _showHideCanvas;

		private const string LoadingText = "Loading";
		private const string TapOnScreenText = "Tap to Play";

		public bool IsShown => _showHideCanvas.IsShown;
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

		public void Hide() =>
			_showHideCanvas.Hide();

		public void SetTapOnTheScreenText() =>
			_text.SetText(TapOnScreenText);

		private void SetLoadingText() =>
			_text.SetText(LoadingText);
	}
}