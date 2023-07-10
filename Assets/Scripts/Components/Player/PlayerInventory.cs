using System;
using Components.Level;
using UnityEngine;

namespace Components.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		private int _erasers;
		private int _pencils;
		private int _fireballs;

		public int erasers
		{
			get => _erasers;
			private set
			{
				_erasers = value;
				OnEraserCountModified?.Invoke(value);
			}
		}
		public int pencils
		{
			get => _pencils;
			private set
			{
				_pencils = value;
				OnPencilCountModified?.Invoke(value);
			}
		}
		public int fireballs
		{
			get => _fireballs;
			private set
			{
				_fireballs = value;
				OnFireballsCountModified?.Invoke(value);
			}
		}

		public event Action<int> OnFireballsCountModified;
		public event Action<int> OnEraserCountModified;
		public event Action<int> OnPencilCountModified;

		[ContextMenu("Place eraser")]
		public void PlaceEraser()
		{
			PlaceItem(CollectableItem.Eraser);
		}
		
		[ContextMenu("Remove eraser")]
		public void RemoveEraser()
		{
			Debug.Log(TryRemoveItem(CollectableItem.Eraser));
		}
		
		public void PlaceItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser)
				erasers++;
			if (item == CollectableItem.Pencil)
				pencils++;
			if (item == CollectableItem.Fireball)
				fireballs++;
		}

		public bool TryRemoveItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser && erasers <= 0)
				return false;
			if (item == CollectableItem.Pencil && pencils <= 0)
				return false;
			if (item == CollectableItem.Fireball && fireballs <= 0)
				return false;
			
			RemoveItem(item);
			return true;
		}
		
		private void RemoveItem(CollectableItem item)
		{
			if (item == CollectableItem.Eraser)
				erasers--;
			if (item == CollectableItem.Pencil)
				pencils--;
			if (item == CollectableItem.Fireball)
				fireballs--;
		}
	}
}