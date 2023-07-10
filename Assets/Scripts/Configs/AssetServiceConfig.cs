using Components;
using Components.Level.Draw;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "AssetConfig", menuName = "Configs/Asset config")]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		public DrawingHand DrawingHand;
		public GameObject Player;
		public Projectile FireballProjectile;
		public Projectile PencilProjectile;
		public Transform PortalAttack;
	}
}