using UnityEngine;

namespace Components.Enemies
{
	public interface IAttackBehavior
	{
		public void PerformAttack(GameObject target);
	}
}