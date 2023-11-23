using Characters.Player;
using UnityEngine;
using Utils;

[RequireComponent(typeof(CircleCollider2D))]
public sealed class HealthBonus : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (!other.TryGetComponent(out PlayerHealth playerHealth))
			return;

		if (playerHealth.IsFullHealth)
			return;
		
		playerHealth.Heal();
		gameObject.SetActive(false);
	}
}