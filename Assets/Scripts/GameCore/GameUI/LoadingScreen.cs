using System;
using TMPro;
using UnityEngine;

namespace GameCore.GameUI
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;
		private ShowHideCanvasGroup _showHideCanvas;
		
		private const string LoadingText = "Loading";
		private const string TapOnScreenText = "Tap the Screen to Play";

		public bool IsShown => _showHideCanvas.IsShown;
		public Action OnShown
		{
			get => _showHideCanvas.OnShown;
			set => _showHideCanvas.OnShown = value;
		}

		private void Awake() =>
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();

		private void Start() =>
			Show();

		private void OnEnable() =>
			_showHideCanvas.OnHided += SetLoadingText;

		private void OnDisable() =>
			_showHideCanvas.OnHided -= SetLoadingText;

		public void Show() =>
			_showHideCanvas.Show();

		public void Hide() =>
			_showHideCanvas.Hide();

		public void SetTapOnTheScreenText() =>
			_text.text = TapOnScreenText;

		private void SetLoadingText() =>
			_text.text = LoadingText;
	}
}