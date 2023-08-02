using System;
using System.Collections;
using UnityEngine;

namespace GameCore.GameUI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ShowHideCanvasGroup : MonoBehaviour
	{
		[SerializeField] private float _showTime;
		[SerializeField] private bool _hideWhenFullyShown;
		private Coroutine _currentRoutine;
		private CanvasGroup _canvasGroup;

		public bool IsShown { get; private set; }
		public Action OnShown;
		public Action OnHided;

		private void Awake() =>
			_canvasGroup = GetComponent<CanvasGroup>();

		[ContextMenu("Show")]
		public void Show() =>
			SetNewRoutine(StartCoroutine(ShowRoutine()));

		[ContextMenu("Hide")]
		public void Hide() =>
			SetNewRoutine(StartCoroutine(HideRoutine()));

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

		private IEnumerator ShowRoutine()
		{
			if (_canvasGroup.alpha >= 1)
				yield break;

			SetInteractable(true);
			SetBlockRaycasts(true);

			while (_canvasGroup.alpha < 1)
			{
				_canvasGroup.alpha += _showTime * Time.unscaledDeltaTime;
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
				_canvasGroup.alpha -= _showTime * Time.unscaledDeltaTime;
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