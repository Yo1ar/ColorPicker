using UnityEngine;

namespace CustomAnimator
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteStateAnimation : MonoBehaviour
	{
		[SerializeField] private CustomAnimationState[] _animationStates;
		private SpriteRenderer _spriteRenderer;
		private CustomAnimationState _currentAnimationState;

		private void Awake() =>
			_spriteRenderer = GetComponent<SpriteRenderer>();

		public void SetState(string stateName)
		{
			if (_currentAnimationState?.name == stateName)
				return;

			_currentAnimationState = FindState(stateName);
			_spriteRenderer.sprite = _currentAnimationState.sprite;
		}

		private CustomAnimationState FindState(string stateName)
		{
			foreach (CustomAnimationState state in _animationStates)
				if (state.name == stateName)
					return state;

			UnityEngine.Debug.LogError("Can't find state name", this);
			return null;
		}
	}
}