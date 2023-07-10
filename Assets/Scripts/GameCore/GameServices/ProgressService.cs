using System.Threading.Tasks;

namespace GameCore.GameServices
{
	public class ProgressService : ServiceBase
	{
		public override async Task InitService()
		{
			await Task.Delay(0);
		}
	}
}