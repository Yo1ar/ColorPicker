using GameCore;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
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
			PlaceItem(EPlayerSkills.Eraser);

		[ContextMenu("Remove eraser")]
		public void RemoveEraser() =>
			TryRemoveItem(EPlayerSkills.Eraser);

		public void PlaceItem(EPlayerSkills item)
		{
			if (item == EPlayerSkills.Eraser)
				Erasers++;
			if (item == EPlayerSkills.Fireball)
				Fireballs++;

			Game.GameLogger.GameLoopLog("Inventory: " + item.ToString() + " placed", this);
		}

		public bool TryRemoveItem(EPlayerSkills item)
		{
			if (item == EPlayerSkills.Eraser && Erasers <= 0)
				return false;
			if (item == EPlayerSkills.Fireball && Fireballs <= 0)
				return false;

			RemoveItem(item);

			Game.GameLogger.GameLoopLog("Inventory: " + item.ToString() + " removed", this);
			return true;
		}

		private void RemoveItem(EPlayerSkills item)
		{
			if (item == EPlayerSkills.Eraser)
				Erasers--;
			if (item == EPlayerSkills.Fireball)
				Fireballs--;
		}

		public bool HaveEraser() =>
			Erasers > 0;
	}
}