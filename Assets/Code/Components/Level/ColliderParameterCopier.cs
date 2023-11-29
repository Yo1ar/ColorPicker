using UnityEngine;

namespace Level
{
	[ExecuteInEditMode]
	public sealed class ColliderParameterCopier : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _fromSize;
		[SerializeField] private BoxCollider2D _toSize;
		[SerializeField] private float _hBoundsOffset = 1;
		[SerializeField] private float _vBoundsOffset = 1;

		private void Awake()
		{
			CopySize();
		}

		private void Update()
		{
#if UNITY_EDITOR
			CopySize();
#endif
		}

		private void CopySize() =>
			_toSize.size = _fromSize.size + new Vector2(_hBoundsOffset, _vBoundsOffset);
	}
}