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
		[SerializeField] private float _floatHeightRandom;
		[SerializeField] private float _floatSpeedRandom;
		[SerializeField] private Ease _ease = Ease.InOutSine;

		private Vector2 _startPosition;
		private Sequence _sequence;
		private float _topBoarder;
		private float _bottomBoarder;

		private void Awake()
		{
			_bottomBoarder = transform.position.y;
			_topBoarder = _bottomBoarder + _floatHeight + Random.Range(0, _floatHeightRandom);
		}

		private void OnEnable() =>
			_sequence = PlayTweenSequence();

		private void OnDisable()
		{
			_sequence.Kill();
			transform.position = new Vector3(transform.position.x, _bottomBoarder);
		}

		private Sequence PlayTweenSequence() =>
			DOTween.Sequence()
				.SetLoops(-1)
				.Append(TweenY(_topBoarder))
				.Append(TweenY(_bottomBoarder))
				.Play();

		private TweenerCore<Vector3, Vector3, VectorOptions> TweenY(float boarder) =>
			transform.DOMoveY(boarder, _floatSpeed + Random.Range(0, _floatSpeedRandom)).SetEase(_ease);
	}
}