using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.GameUI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ShowHideCanvasGroup : MonoBehaviour
	{
		[SerializeField] private float _showTime;
		[SerializeField] private float _hideTime;
		[SerializeField] private bool _unscaled;
		
		private CanvasGroup _canvasGroup;
		private Tween _tween;
		public UnityEvent OnShown = new();
		public UnityEvent OnHided = new();

		public bool IsShown { get; private set; }

		private void Awake() =>
			_canvasGroup = GetComponent<CanvasGroup>();

		public void Show()
		{
			if (_tween.IsActive())
				_tween.onComplete += ShowAction;
			else
				ShowAction();
		}

		public void Hide()
		{
			if (_tween.IsActive())
				_tween.onComplete += HideAction;
			else
				HideAction();
		}

		private void ShowAction()
		{
			SetInteractable(true);
			SetBlockRaycasts(true);

			_tween.Kill();

			if (_unscaled)
				_tween = _canvasGroup.DOFade(1, _showTime).SetUpdate(true);
			else
				_tween = _canvasGroup.DOFade(1, _showTime);
			
			_tween.onComplete += InvokeShown;

			void InvokeShown()
			{
				OnShown?.Invoke();
				IsShown = true;
			}
		}

		private void HideAction()
		{
			
			SetInteractable(false);
			SetBlockRaycasts(false);
			
			_tween.Kill();
			
			if (_unscaled)
				_tween = _canvasGroup.DOFade(0, _hideTime).SetUpdate(true);
			else
				_tween = _canvasGroup.DOFade(0, _hideTime);

			_tween.onComplete += InvokeHided;
			
			void InvokeHided()
			{
				OnHided?.Invoke();
				IsShown = false;
			}
		}

		private void SetBlockRaycasts(bool value) =>
			_canvasGroup.blocksRaycasts = value;

		private void SetInteractable(bool value) =>
			_canvasGroup.interactable = value;

		[ContextMenu("Show")]
		private void ContextShow()
		{
			if (!_canvasGroup)
				_canvasGroup = GetComponent<CanvasGroup>();
			
			_canvasGroup.alpha = 1;
			SetInteractable(true);
			SetBlockRaycasts(true);
		}
		[ContextMenu("Hide")]
		private void ContextHide()
		{
			if (!_canvasGroup)
				_canvasGroup = GetComponent<CanvasGroup>();
			
			_canvasGroup.alpha = 0;
			SetInteractable(false);
			SetBlockRaycasts(false);
		}
	}
}