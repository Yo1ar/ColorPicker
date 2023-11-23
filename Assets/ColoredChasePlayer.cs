using UnityEngine;
using Utils;
using Utils.Constants;

public sealed class ColoredChasePlayer : ColorCheckerBase
{
	[Header("Chase")] [SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private float _chaseStopDistance = 1f;
	[SerializeField] private float _chaseSpeed = 3f;

	private Transform _transform;
	private Transform _targetTransform;
	private Vector2 _startingPoint;
	private bool _isChasing;
	private bool _onPlace;

	protected override void Awake()
	{
		base.Awake();
		_transform = transform;
		_startingPoint = _transform.position;
	}

	private void FixedUpdate()
	{
		if (_onPlace)
			return;

		if (_isChasing)
		{
			Debug.Log("Chasing");
			Follow(_targetTransform.position);
		}
		else
		{
			Debug.Log("Backing");
			Follow(_startingPoint);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!CanStartChasing(other))
			return;

		if (_isChasing)
			return;

		_targetTransform = other.transform;
		_isChasing = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;
		
		_isChasing = false;
		_targetTransform = null;
	}

	private void Follow(Vector3 targetPosition)
	{
		var distance = Vector3.Distance(targetPosition, _transform.position);
		if (distance < _chaseStopDistance)
		{
			_onPlace = true;
			return;
		}

		Debug.Log("Moving");
		Vector3 direction = targetPosition - _transform.position;
		_rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * _chaseSpeed;
	}

	private bool CanStartChasing(Collider2D other) =>
		other.IsPlayer() && IsSameColor(PlayerColorHolder);

	#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Colors.GetColor(Color);
		Gizmos.DrawWireSphere(transform.position, _chaseStopDistance);
	}
	#endif
}