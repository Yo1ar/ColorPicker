using UnityEngine;
using UnityEngine.Pool;

namespace Components.Level.Draw
{
	public sealed class DrawingHand : MonoBehaviour
	{
		private IObjectPool<DrawingHand> _objectPool;
		private Animator _animator;

		private void Awake() => 
			_animator = GetComponent<Animator>();

		private void OnEnable() => 
			_animator.Play("Draw");

		public void SetPool(IObjectPool<DrawingHand> objectPool) =>
			_objectPool = objectPool;

		public void OnAnimationFinish() => 
			_objectPool?.Release(this);
	}
}