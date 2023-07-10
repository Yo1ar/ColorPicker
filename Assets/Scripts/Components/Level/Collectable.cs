using Components.Player;
using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Components.Level
{
	public class Collectable : MonoBehaviour
	{
		[SerializeField] private CollectableItem _item;
		private PlayerInventory _playerInventory;

		private void Awake() => 
			gameObject.tag = Tags.Collectable;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			var inv = other.GetComponent<PlayerInventory>();
			inv.PlaceItem(_item);
			gameObject.SetActive(false);
		}
	}
}