using System;
using System.Collections;
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

		public ShowHideUiHandler(VisualElement visualElement, MonoBehaviour monoBehaviour, Action<bool> onChanged = null, bool hideImmediate = false)
		{
			if (visualElement == null || monoBehaviour == null)
				Debug.LogError("Can't init members with Null");

			_visualElement = visualElement;
			_onChanged = onChanged;

			if (hideImmediate)
				monoBehaviour.StartCoroutine(SetDisplayNone());
		}

		public async void Show() =>
			await ShowAwaitable();

		public async void Hide() =>
			await HideAwaitable();

		public async Task ShowAwaitable()
		{
			DisplayImmediate(true);

			_visualElement.RemoveFromClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (!_isShown)
				await Task.Yield();
		}

		public async Task HideAwaitable()
		{
			_visualElement.AddToClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (_isShown)
				await Task.Yield();

			DisplayImmediate(false);
		}

		public async void Toggle()
		{
			if (_isShown)
				await HideAwaitable();
			else
				await ShowAwaitable();
		}

		public async Task ToggleAwaitable()
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

		private IEnumerator SetDisplayNone()
		{
			yield return null;
			yield return null;
			_visualElement.style.display = DisplayStyle.None;
		}

		private void DisplayImmediate(bool value) =>
			_visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
	}
}