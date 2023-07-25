using UnityEngine;

namespace GameCore.GameUI
{
	public class LoadingScreen : MonoBehaviour
	{
		private ShowHideCanvasGroup _showHideCanvas;
		
		private void Awake() =>
			_showHideCanvas = GetComponent<ShowHideCanvasGroup>();

		public void Show() =>
			_showHideCanvas.Show();

		public void Hide() =>
			_showHideCanvas.Hide();
		
	}
}