using UnityEngine;
using Utils;
using Utils.Constants;

public sealed class ColoredChasePlayer : ColorCheckerBase
{
	[Header("Chase")] [SerializeField] private EnemyMoveBehaviour _enemyMoveBehaviour;
	[SerializeField] private float _targetReachedDistance = 1f;

	private Vector2 _startingPoint;
	private bool _isChasing = false;
	private Transform _target = null;

	protected override void Awake()
	{
		base.Awake();
		_startingPoint = transform.position;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!CanChase(other))
			return;

		SetCanChase();
		
		PlayerColorHolder.OnColorChanged += SetColor;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(PlayerColorHolder))
			SetCanChase();
		else
			SetStopChase();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		SetStopChase();
		PlayerColorHolder.OnColorChanged -= SetColor;
	}

	private void Update()
	{
		if (_isChasing)
		{
			Vector3 direction = GetDirection(to: _target.position);

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

	private void SetCanChase()
	{
		_target = PlayerColorHolder.transform;
		_isChasing = true;
	}

	private void SetStopChase()
	{
		_isChasing = false;
		_target = null;
	}

	private bool TargetReached(float distanceToTarget, float overrideReachedDistance = 0f)
	{
		if (overrideReachedDistance == 0)
			return distanceToTarget <= _targetReachedDistance;
		else
			return distanceToTarget <= overrideReachedDistance;
	}

	private bool CanChase(Collider2D other) =>
		other.IsPlayer() && IsSameColor(PlayerColorHolder);

	private Vector3 GetDirection(Vector3 to) =>
		to - transform.position;

	#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Colors.GetColor(Color);
		Gizmos.DrawWireSphere(transform.position, _targetReachedDistance);
	}
	#endif
}