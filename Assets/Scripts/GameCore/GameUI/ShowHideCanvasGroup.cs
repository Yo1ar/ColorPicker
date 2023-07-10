using System.Collections;
using UnityEngine;

namespace GameCore.GameUI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ShowHideCanvasGroup : MonoBehaviour
	{
		[SerializeField] private float _showTime;
		private Coroutine _currentRoutine;
		private CanvasGroup _canvasGroup;

		public bool IsShown { get; private set; }

		private void Awake() =>
			_canvasGroup = GetComponent<CanvasGroup>();

		[ContextMenu("Show")]
		public void Show() =>
			SetNewRoutine(StartCoroutine(ShowRoutine()));

		[ContextMenu("Hide")]
		public void Hide() =>
			SetNewRoutine(StartCoroutine(HideRoutine()));

		private IEnumerator ShowRoutine()
		{
			if (_canvasGroup.alpha >= 1)
				yield break;

			SetInteractable(true);
			SetBlockRaycasts(true);

			IsShown = true;

			while (_canvasGroup.alpha < 1)
			{
				_canvasGroup.alpha += _showTime * Time.unscaledDeltaTime;
				yield return null;
			}
		}

		private IEnumerator HideRoutine()
		{
			if (_canvasGroup.alpha <= 0)
				yield break;

			SetInteractable(false);
			SetBlockRaycasts(false);

			IsShown = false;

			while (_canvasGroup.alpha > 0)
			{
				_canvasGroup.alpha -= _showTime * Time.unscaledDeltaTime;
				yield return null;
			}
		}

		private void SetBlockRaycasts(bool value) =>
			_canvasGroup.blocksRaycasts = value;

		private void SetInteractable(bool value) =>
			_canvasGroup.interactable = value;

		private void SetNewRoutine(Coroutine value)
		{
			StopCurrentRoutine();
			_currentRoutine = value;
		}

		private void StopCurrentRoutine()
		{
			if (_currentRoutine == null)
				return;

			StopCoroutine(_currentRoutine);
			_currentRoutine = null;
		}
	}
}