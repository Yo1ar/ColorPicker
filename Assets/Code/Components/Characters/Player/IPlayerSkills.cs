using Utils;

namespace Characters.Player
{
	public interface IPlayerSkills
	{
		Cooldown FireballCooldown { get; }
		Cooldown EraserCooldown { get; }
		bool IsAttacking { get; }
		bool ErasableMode { get; }
		void ShootFireball();
		void SwitchErasableMode();
	}
}