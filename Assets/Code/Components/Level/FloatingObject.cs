using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Level
{
	public class FloatingObject : MonoBehaviour
	{
		[SerializeField] private float _floatHeight = 0.5f;
		[SerializeField] private float _floatSpeed = 1f;
		[SerializeField] private Ease _ease = Ease.InOutSine;

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
			_topBoarder = _bottomBoarder + _floatHeight;

		private Sequence PlayTweenSequence() =>
			DOTween.Sequence()
				.SetLoops(-1)
				.Append(TweenY(_topBoarder))
				.Append(TweenY(_bottomBoarder))
				.Play();

		private void OnDisable() => 
			_sequence.Kill();

		private TweenerCore<Vector3, Vector3, VectorOptions> TweenY(float boarder) =>
			transform.DOMoveY(boarder, _floatSpeed).SetEase(_ease);
	}
}