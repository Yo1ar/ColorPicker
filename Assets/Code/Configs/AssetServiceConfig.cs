using GameCore.GameServices;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "AssetConfig", menuName = "Configs/Asset config")]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		public GameObject Player;
		public Projectile FireballProjectile;
		public Projectile DropProjectile;
		public AudioSourcesController AudioSourcesController;
	}
}