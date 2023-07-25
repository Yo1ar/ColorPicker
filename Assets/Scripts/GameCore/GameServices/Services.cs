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

		public Services() => 
			CreateServices();

		private void CreateServices()
		{
			InputService = new();
			ConfigService = new();
			ProgressService = new();
			AssetService = new();
			FactoryService = new();
		}

		public async Task InitServices()
		{
			await InputService.InitService();
			await ConfigService.InitService();
			await ProgressService.InitService();
			await AssetService.InitService();
			await FactoryService.InitService();
		}
	}
}