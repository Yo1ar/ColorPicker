using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class ShowHideUiHandler
	{
		private const string k_opacity0 = "opacity-0";

		private readonly VisualElement _visualElement;
		private readonly Action<bool> _onChanged;
		private bool _isShown = false;

		public bool IsShown => _isShown;

		public ShowHideUiHandler(VisualElement visualElement, Action<bool> onChanged = null)
		{
			if (visualElement == null)
			{
				Debug.LogError("Can't init visual element with Null");
				return;
			}
			
			_visualElement = visualElement;
			_onChanged = onChanged;
		}

		public async void Show() =>
			await ShowAwaitable();

		public async void Hide() =>
			await HideAwaitable();

		public async void Toggle() =>
			await ToggleAwaitable();

		private async Task ShowAwaitable()
		{
			_visualElement.BringToFront();
			_visualElement.RemoveFromClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (!_isShown)
				await Task.Yield();
		}

		private async Task HideAwaitable()
		{
			_visualElement.SendToBack();
			_visualElement.AddToClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (_isShown)
				await Task.Yield();
		}

		private async Task ToggleAwaitable()
		{
			if (_isShown)
				await HideAwaitable();
			else
				await ShowAwaitable();
		}

		private void OnTransitionEnded(TransitionEndEvent evt)
		{
			_visualElement.UnregisterCallback<TransitionEndEvent>(OnTransitionEnded);
			_isShown = !_visualElement.ClassListContains(k_opacity0);
			_onChanged?.Invoke(_isShown);
		}
	}
}