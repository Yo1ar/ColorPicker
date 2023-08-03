using System.Threading.Tasks;

namespace GameCore.GameServices
{
	public sealed class Services
	{
		public static ConfigService ConfigService { get; private set; }
		public static ProgressService ProgressService { get; private set; }
		public static AssetService AssetService { get; private set; }
		public static FactoryService FactoryService { get; private set; }
		public static InputService InputService { get; private set; }

		public async Task InitServices()
		{
			ConfigService = new ConfigService();
			await ConfigService.InitService();
			
			InputService = new();
			await InputService.InitService();

			ProgressService = new();
			await ProgressService.InitService();

			AssetService = new(ConfigService.AssetServiceConfig);
			await AssetService.InitService();

			FactoryService = new(AssetService);
			await FactoryService.InitService();
		}
	}
}