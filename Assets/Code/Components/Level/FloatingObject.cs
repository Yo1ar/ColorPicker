using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Components.Level
{
	public class FloatingObject : MonoBehaviour
	{
		private const float FLOAT_HEIGHT = 0.5f;
		private const float FLOAT_SPEED = 1f;
		private const Ease EASE = Ease.InOutSine;

		private Sequence _sequence;
		private float _topBoarder;
		private float _bottomBoarder;

		private void Awake()
		{
			_bottomBoarder = transform.position.y;
			SetTopBoarder();
		}

		private void Start() => 
			_sequence = PlayTweenSequence();

		private void SetTopBoarder() =>
			_topBoarder = _bottomBoarder + FLOAT_HEIGHT;

		private Sequence PlayTweenSequence() =>
			DOTween.Sequence()
				.SetLoops(-1)
				.Append(TweenY(_topBoarder))
				.Append(TweenY(_bottomBoarder))
				.Play();

		private void OnDisable() => 
			_sequence.Kill();

		private TweenerCore<Vector3, Vector3, VectorOptions> TweenY(float boarder) =>
			transform.DOMoveY(boarder, FLOAT_SPEED).SetEase(EASE);
	}
}