using UnityEngine;

namespace Enemies
{
	public interface IAttackBehavior
	{
		public void PerformAttack(GameObject target);
	}
}