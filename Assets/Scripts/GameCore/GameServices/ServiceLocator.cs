using System.Threading.Tasks;

namespace GameCore.GameServices
{
	public sealed class ServiceLocator
	{
		public static ConfigService configService { get; private set; }
		public static ProgressService progressService { get; private set; }
		public static AssetService assetService { get; private set; }
		public static FactoryService factoryService { get; set; }

		public ServiceLocator() => 
			CreateServices();

		private void CreateServices()
		{
			configService = new();
			progressService = new();
			assetService = new();
			factoryService = new();
		}

		public async Task InitServices()
		{
			await configService.InitService();
			await progressService.InitService();
			await assetService.InitService();
			await factoryService.InitService();
		}
	}
}