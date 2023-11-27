using GameCore.GameServices;
using UnityEngine;
using Utils;

public sealed class EnemyRangedAttack : MonoBehaviour
{
	[SerializeField] private ColorCheckerBase _colorChecker;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _attackCooldownTime;

	private readonly int _attackHash = Animator.StringToHash("attack");
	private Cooldown _cooldown;
	private ColorHolderBase _playerColorHolder;
	private FactoryService _factoryService;

	private void Awake()
	{
		_factoryService = Services.FactoryService;
		_cooldown = new Cooldown(_attackCooldownTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (!other.TryGetComponent(out _playerColorHolder) || !_colorChecker.IsSameColor(_playerColorHolder))
			return;

		if (!_cooldown.IsReady)
			return;

		_animator.Play(_attackHash);
		_cooldown.Reset();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (!_colorChecker.IsSameColor(_playerColorHolder))
			return;

		if (!_cooldown.IsReady)
			return;
		
		_animator.Play(_attackHash);
		_cooldown.Reset();
	}

	private void Anim_Attack() =>
		_factoryService.CreateDrop(transform.position, Vector2.down, Vector3.zero);
}