using UnityEngine;

namespace Characters.Player
{
	public class GroundCheck : MonoBehaviour
	{
		[SerializeField] private LayerMask _groundLayers;
		[Range(0, 0.3f), SerializeField] private float _groundCheckDepth;
		[Range(0, 1f), SerializeField] private float _groundCheckWide;
		private CapsuleCollider2D _collider2D;
		private Bounds _colliderBounds;
		private readonly RaycastHit2D[] _raycastResults = new RaycastHit2D[1];

		public bool IsGrounded { get; private set; }

		private void Awake() =>
			_collider2D = GetComponent<CapsuleCollider2D>();

		private void Update() =>
			CheckGrounded();

		private void CheckGrounded()
		{
			_colliderBounds = _collider2D.bounds;
			float groundCheckDepth = -_colliderBounds.extents.y - _groundCheckDepth * 0.5f;
			int groundCount = GetGroundCount(
				origin: new Vector2(_colliderBounds.center.x, _colliderBounds.center.y + groundCheckDepth),
				size: new Vector2(_collider2D.size.x * _groundCheckWide, _groundCheckDepth));

			IsGrounded = groundCount > 0;
		}

		private int GetGroundCount(Vector2 origin, Vector2 size) =>
			Physics2D.BoxCastNonAlloc(origin, size, 0f, Vector2.down, _raycastResults, 0f, _groundLayers);

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			_collider2D = GetComponent<CapsuleCollider2D>();
			Bounds bounds = _collider2D.bounds;

			Gizmos.color = IsGrounded ? Color.green : Color.red;

			Gizmos.DrawCube(
				center: new Vector3(bounds.center.x, bounds.center.y - bounds.extents.y - _groundCheckDepth * 0.5f),
				size: new Vector3(_collider2D.size.x * _groundCheckWide, _groundCheckDepth));
		}
#endif
	}
}