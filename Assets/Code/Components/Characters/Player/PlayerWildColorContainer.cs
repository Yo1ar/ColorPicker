using System;
using UnityEngine;

namespace Characters.Player
{
	public class PlayerWildColorContainer : MonoBehaviour, IWildColorContainer
	{
		public int WildColorBonusCount { get; private set; } = 5;

		public void SetWldColorBonus(int value) =>
			WildColorBonusCount = value;

		public event Action<int> OnCountChange;

		public void AddWildColorBonus()
		{
			WildColorBonusCount++;
			OnCountChange?.Invoke(WildColorBonusCount);
		}

		public bool TryUseWildColorBonus()
		{
			if (WildColorBonusCount - 1 < 0)
				return false;

			WildColorBonusCount--;
			OnCountChange?.Invoke(WildColorBonusCount);
			return true;
		}

		public bool CanUseWildColorBonus() =>
			WildColorBonusCount >= 1;
	}
}