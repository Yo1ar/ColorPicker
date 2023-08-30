using Components.Level;
using GameCore;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		private int _erasers;
		private int _pencils;
		private int _fireballs;

		public int Erasers
		{
			get => _erasers;
			private set
			{
				_erasers = value;
				OnEraserCountModified?.Invoke(value);
			}
		}

		public int Fireballs
		{
			get => _fireballs;
			private set
			{
				_fireballs = value;
				OnFireballsCountModified?.Invoke(value);
			}
		}

		public UnityEvent<int> OnFireballsCountModified;
		public UnityEvent<int> OnEraserCountModified;

		[ContextMenu("Place eraser")]
		public void PlaceEraser() =>
			PlaceItem(CollectableItem.Eraser);

		[ContextMenu("Remove eraser")]
		public void RemoveEraser() =>
			TryRemoveItem(CollectableItem.Eraser);

		public void PlaceItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser)
				Erasers++;
			if (item == CollectableItem.Fireball)
				Fireballs++;

			Game.GameLogger.GameLoopLog("Inventory: " + item.ToString() + " placed", this);
		}

		public bool TryRemoveItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser && Erasers <= 0)
				return false;
			if (item == CollectableItem.Fireball && Fireballs <= 0)
				return false;

			RemoveItem(item);

			Game.GameLogger.GameLoopLog("Inventory: " + item.ToString() + " removed", this);
			return true;
		}

		private void RemoveItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser)
				Erasers--;
			if (item == CollectableItem.Fireball)
				Fireballs--;
		}

		public bool HaveEraser() =>
			Erasers > 0;
	}
}