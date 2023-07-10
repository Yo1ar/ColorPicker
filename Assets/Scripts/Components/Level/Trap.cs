using System.Collections;
using Components.CustomAnimator;
using Components.Player;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Components.Level
{
	public class Trap : MonoBehaviour
	{
		[SerializeField] private float _reloadTime;

		private SpriteStateAnimation _spriteStateAnimation;
		private WaitForSeconds _waitForSeconds;
		private bool _isReloading;

		private void Awake() => 
			_spriteStateAnimation = GetComponentInChildren<SpriteStateAnimation>();

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!CanAttack(other))
				return;

			TryDamage(other);
		}

		private void TryDamage(Collider2D other)
		{
			if (!other.TryGetComponent(out PlayerHealth health))
				return;

			health.Damage();
			_spriteStateAnimation.SetState("attack");
			_isReloading = true;
		
			StartCoroutine(ReloadTrap());
		}

		private bool CanAttack(Collider2D other) =>
			other.IsPlayer() && !_isReloading;

		private IEnumerator ReloadTrap()
		{
			yield return new WaitForSeconds(_reloadTime);
			_spriteStateAnimation.SetState("idle");
			_isReloading = false;
		}
	}
}