using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Utils.Constants;

namespace Components
{
	[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
	public sealed class Projectile : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private GameTag _targetTag;
		[SerializeField] private float _releaseAfter = 1.5f;
		private Rigidbody2D _rigidbody;
		private CapsuleCollider2D _capsuleCollider2D;
		private IObjectPool<Projectile> _pool;
		private Coroutine _releaseRoutine;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
			_capsuleCollider2D.isTrigger = true;
			_rigidbody.gravityScale = 0f;
		}

		private void OnEnable() =>
			_releaseRoutine = StartCoroutine(ReleaseRoutine());

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag(_targetTag.ToString()) ||
			    !other.gameObject.TryGetComponent(out IHealth health))
				return;

			health.Damage();
			Release();
		}

		public void Launch(Vector2 direction)
		{
			_rigidbody.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			_rigidbody.AddForce(direction * _speed);
		}

		public void SetPool(IObjectPool<Projectile> fireballPool) =>
			_pool = fireballPool;

		private IEnumerator ReleaseRoutine()
		{
			yield return new WaitForSeconds(_releaseAfter);

			_pool.Release(this);
		}

		private void Release()
		{
			_pool.Release(this);

			if (_releaseRoutine != null)
				StopCoroutine(_releaseRoutine);
		}
	}
}