using UnityEngine;
using Utils;
using Utils.Constants;

public class ChaseComponent : MonoBehaviour
{
	[SerializeField] private Collider2D _collider;
	[SerializeField] private MoveComponent _moveComponent;
	[SerializeField] private GameTag _tagToChase;
	[SerializeField] private float _chaseDelay;
	[SerializeField] private float _chaseDelayRecastTime;
	
	private Cooldown _chaseDelayCooldown;
	private Cooldown _chaseDelayRecastCooldown;
	
	private void Awake()
	{
		if (!_moveComponent)
			UnityEngine.Debug.LogError("_moveComponent is empty");

		_chaseDelayCooldown = new Cooldown(_chaseDelay);
		_chaseDelayCooldown.Reset();

		_chaseDelayRecastCooldown = new Cooldown(_chaseDelayRecastTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_chaseDelayRecastCooldown.IsReady)
			_chaseDelayCooldown.Reset();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		_moveComponent.MoveDirection = 0;
		_chaseDelayRecastCooldown.Reset();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!ShouldChase(other))
			return;
		
		Chase(other);
	}

	private bool ShouldChase(Collider2D other) =>
		_chaseDelayCooldown.IsReady && other.CompareTag(_tagToChase.ToString());

	private void Chase(Collider2D other) =>
		_moveComponent.MoveDirection = GetChaseDirection(other);

	private float GetChaseDirection(Collider2D target)
	{
		Vector3 direction = target.transform.position - transform.parent.position;
		return direction.x;
	}
}
