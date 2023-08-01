using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		public string Name;
		public GameObject Prefab;
	}
}