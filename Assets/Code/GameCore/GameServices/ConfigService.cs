using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public class ConfigService : ServiceBase
	{
		public ScenesConfig ScenesConfig { get; private set; }
		public AssetServiceConfig AssetServiceConfig { get; private set; }

		public override async Task InitService()
		{
			ScenesConfig = await LoadConfig<ScenesConfig>();
			AssetServiceConfig = await LoadConfig<AssetServiceConfig>();
			
			Game.GameLogger.GameLog("Initialized", this);
		}

		private async Task<TConfig> LoadConfig<TConfig>() where TConfig : ScriptableObject
		{
			ResourceRequest request = Resources.LoadAsync<TConfig>(GetConfigPath<TConfig>());

			while (!request.isDone)
				await Task.Yield();

			return request.asset as TConfig;
		}

		private static string GetConfigPath<TConfig>() where TConfig : ScriptableObject =>
			typeof(TConfig).ToString().Replace('.', '/');
	}
}