using System;
using UnityEngine;

namespace Characters.Player
{
	public class PlayerSkills : MonoBehaviour
	{
		private int _erasers;
		private int _fireballs;
		public event Action SkillsUpdated;

		public void AddSkillStack(EPlayerSkills skill)
		{
			switch (skill)
			{
				case EPlayerSkills.Eraser:
					_erasers += 1;
					break;
				case EPlayerSkills.Fireball:
					_fireballs += 1;
					break;
				default:
					throw new NotImplementedException();
			}

			SkillsUpdated?.Invoke();
		}
		public void RemoveSkillStack(EPlayerSkills skill)
		{
			switch (skill)
			{
				case EPlayerSkills.Eraser:
					_erasers -= 1;
					break;
				case EPlayerSkills.Fireball:
					_fireballs -= 1;
					break;
				default:
					throw new NotImplementedException();
			}

			SkillsUpdated?.Invoke();
		}
	}

	public enum EPlayerSkills
	{
		Eraser,
		Fireball
	}
}