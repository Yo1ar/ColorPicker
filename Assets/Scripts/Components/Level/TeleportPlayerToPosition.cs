using UnityEngine;

namespace Components.Level
{
	public class TeleportPlayerToPosition : MonoBehaviour
	{
		[SerializeField] private Transform _target;

		public void Teleport(Transform player)
		{
			if (!player.TryGetComponent(out Rigidbody2D rigidbody2D))
				return;
		
			player.position = _target.position;
			rigidbody2D.velocity = Vector2.zero;
		}
	}
}