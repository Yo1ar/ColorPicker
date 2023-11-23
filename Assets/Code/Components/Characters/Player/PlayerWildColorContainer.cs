using UnityEngine;

namespace Characters.Player
{
	public class PlayerWildColorContainer : MonoBehaviour, IWildColorContainer
	{
		public int WildColorBonus { get; private set; } = 5;

		public void SetWldColorBonus(int value) =>
			WildColorBonus = value;

		public void AddWildColorBonus() =>
			WildColorBonus++;

		public bool TryUseWildColorBonus()
		{
			if (WildColorBonus - 1 < 0)
				return false;

			WildColorBonus--;
			return true;
		}

		public bool CanUseWildColorBonus() =>
			WildColorBonus >= 1;
	}
}