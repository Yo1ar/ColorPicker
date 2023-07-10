using System.Threading.Tasks;
using Components;
using Components.Level.Draw;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public class AssetService : ServiceBase
	{
		private AssetServiceConfig _serviceConfig;
		public DrawingHand DrawingHand => _serviceConfig.DrawingHand;
		public Object Player => _serviceConfig.Player;
		public Projectile FireballProjectile => _serviceConfig.FireballProjectile;
		public Projectile PencilProjectile => _serviceConfig.PencilProjectile;
		public Transform PortalAttack => _serviceConfig.PortalAttack;

		public override Task InitService()
		{
			_serviceConfig = ServiceLocator.configService.assetServiceConfig;
			return Task.CompletedTask;
		}
	}
}