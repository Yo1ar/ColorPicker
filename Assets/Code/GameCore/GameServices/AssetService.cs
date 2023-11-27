using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public class AssetService : ServiceBase
	{
		private readonly AssetServiceConfig _assetServiceConfig;
		public Object Player => _assetServiceConfig.Player;
		public Projectile FireballProjectile => _assetServiceConfig.FireballProjectile;
		public Projectile DropProjectile => _assetServiceConfig.DropProjectile;
		public AudioSourcesController AudioSourcesController => _assetServiceConfig.AudioSourcesController;

		public AssetService(AssetServiceConfig assetServiceConfig) =>
			_assetServiceConfig = assetServiceConfig;

		public override Task InitService()
		{
			Game.GameLogger.GameLog("Initialized", this);
			return Task.CompletedTask;
		}
	}
}