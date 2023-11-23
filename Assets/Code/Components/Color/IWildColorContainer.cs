public interface IWildColorContainer
{
	int WildColorBonus { get; }
	void SetWldColorBonus(int value);
	void AddWildColorBonus();
	bool TrySpendWildColorBonus();
	bool CanUseWildColorBonus();
}