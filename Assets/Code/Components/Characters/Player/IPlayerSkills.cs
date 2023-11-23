namespace Characters.Player
{
	public interface IPlayerSkills
	{
		bool ErasableMode { get; }
		void ShootFireball();
		void SwitchErasableMode();
	}
}