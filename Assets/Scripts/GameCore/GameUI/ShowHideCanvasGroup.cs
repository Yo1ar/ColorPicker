using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.GameUI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ShowHideCanvasGroup : MonoBehaviour
	{
		[SerializeField] private float _showTime;
		[SerializeField] private float _hideTime;
		private CanvasGroup _canvasGroup;
		private Coroutine _currentRoutine;
		public UnityEvent OnShown = new();
		public UnityEvent OnHided = new();

		public bool IsShown { get; private set; }

		private void Awake() =>
			_canvasGroup = GetComponent<CanvasGroup>();

		[ContextMenu("Show")]
		public void Show() =>
			SetNewRoutine(StartCoroutine(ShowRoutine()));

		[ContextMenu("Hide")]
		public void Hide() =>
			SetNewRoutine(StartCoroutine(HideRoutine()));

		private void SetNewRoutine(Coroutine coroutine)
		{
			StopCurrentRoutine();
			_currentRoutine = coroutine;
		}

		private void StopCurrentRoutine()
		{
			if (_currentRoutine == null)
				return;

			StopCoroutine(_currentRoutine);
			_currentRoutine = null;
		}

		private IEnumerator ShowRoutine()
		{
			if (_canvasGroup.alpha >= 1)
				yield break;

			SetInteractable(true);
			SetBlockRaycasts(true);

			while (_canvasGroup.alpha < 1)
			{
				_canvasGroup.alpha += 1 / _showTime * Time.unscaledDeltaTime;
				yield return null;
			}

			IsShown = true;
			OnShown?.Invoke();
		}

		private IEnumerator HideRoutine()
		{
			if (_canvasGroup.alpha <= 0)
				yield break;

			SetInteractable(false);
			SetBlockRaycasts(false);

			while (_canvasGroup.alpha > 0)
			{
				_canvasGroup.alpha -= 1 / _hideTime * Time.unscaledDeltaTime;
				yield return null;
			}

			IsShown = false;
			OnHided?.Invoke();
		}

		private void SetBlockRaycasts(bool value) =>
			_canvasGroup.blocksRaycasts = value;

		private void SetInteractable(bool value) =>
			_canvasGroup.interactable = value;
	}
}