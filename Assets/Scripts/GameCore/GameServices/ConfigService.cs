using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public class ConfigService : ServiceBase
	{
		public ScenesConfig scenesConfig { get; private set; }
		public AssetServiceConfig assetServiceConfig { get; private set; }

		public override async Task InitService()
		{
			scenesConfig = await LoadConfig<ScenesConfig>();
			assetServiceConfig = await LoadConfig<AssetServiceConfig>();
		}

		private async Task<TConfig> LoadConfig<TConfig>() where TConfig : ScriptableObject
		{
			// Debug.Log(typeof(TConfig));
			ResourceRequest request = Resources.LoadAsync<TConfig>(GetConfigPath<TConfig>());

			while (!request.isDone)
				await Task.Yield();

			return request.asset as TConfig;
		}

		private static string GetConfigPath<TConfig>() where TConfig : ScriptableObject =>
			typeof(TConfig).ToString().Replace('.', '/');
	}
}