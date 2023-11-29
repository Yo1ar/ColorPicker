using System;

public interface IWildColorContainer
{
	int WildColorBonusCount { get; }
	void SetWldColorBonus(int value);
	event Action<int> OnCountChange;
	
	void AddWildColorBonus();
	bool TryUseWildColorBonus();
	bool CanUseWildColorBonus();
}