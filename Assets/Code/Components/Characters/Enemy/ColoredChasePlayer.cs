using UnityEngine;
using Utils;
using Utils.Constants;

namespace Characters.Enemy
{
	public sealed class ColoredChasePlayer : MonoBehaviour
	{
		[SerializeField] private EnemyMoveBehaviour _enemyMoveBehaviour;
		[SerializeField] private float _targetReachedDistance = 1f;
		[SerializeField] private ColorCheckerBase _colorChecker;
		[SerializeField] private bool _followTargetColor;

		private Vector2 _startingPoint;
		private bool _isChasing;
		private ColorHolderBase _colorHolder;

		private void Awake() =>
			_startingPoint = transform.position;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			if (!other.TryGetComponent(out _colorHolder))
				return;

			if (!_colorChecker.IsSameColor(_colorHolder))
				return;

			_isChasing = true;

			if (_followTargetColor)
				_colorHolder.OnColorChanged += _colorChecker.SetColor;
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			_isChasing = _colorChecker.IsSameColor(_colorHolder);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			_isChasing = false;

			if (_followTargetColor)
				_colorHolder.OnColorChanged -= _colorChecker.SetColor;
		}

		private void Update()
		{
			if (_isChasing && _colorHolder)
			{
				Vector3 direction = GetDirection(to: _colorHolder.transform.position);

				_enemyMoveBehaviour.SetDirection(
					TargetReached(direction.magnitude)
						? Vector3.zero
						: direction.normalized);
			}
			else
			{
				Vector3 direction = GetDirection(to: _startingPoint);

				_enemyMoveBehaviour.SetDirection(
					TargetReached(direction.magnitude, 0.1f)
						? Vector3.zero
						: direction.normalized);
			}
		}

		private bool TargetReached(float distanceToTarget, float overrideReachedDistance = 0f)
		{
			if (overrideReachedDistance == 0)
				return distanceToTarget <= _targetReachedDistance;
			else
				return distanceToTarget <= overrideReachedDistance;
		}

		private Vector3 GetDirection(Vector3 to) =>
			to - transform.position;

	#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Colors.GetColor(EColors.White);
			Gizmos.DrawWireSphere(transform.position, _targetReachedDistance);
		}
	#endif
	}
}