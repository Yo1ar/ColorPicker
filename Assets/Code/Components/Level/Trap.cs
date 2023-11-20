using System.Collections;
using Components.Player;
using UnityEngine;
using Utils;

namespace Components.Level
{
	public class Trap : MonoBehaviour
	{
		[SerializeField] private float _reloadTime;
		[SerializeField] private Sprite _loadedSprite;
		[SerializeField] private Sprite _triggeredSprite;
		private SpriteRenderer _spriteRenderer;
		private WaitForSeconds _waitForSeconds;
		private bool _isReloading;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = _loadedSprite;
		}

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
			_spriteRenderer.sprite = _triggeredSprite;
			_isReloading = true;
		
			StartCoroutine(ReloadTrap());
		}

		private bool CanAttack(Collider2D other) =>
			other.IsPlayer() && !_isReloading;

		private IEnumerator ReloadTrap()
		{
			yield return new WaitForSeconds(_reloadTime);
			_spriteRenderer.sprite = _loadedSprite;
			_isReloading = false;
		}
	}
}