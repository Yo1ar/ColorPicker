using System.Threading.Tasks;

namespace GameCore.GameServices
{
	public abstract class ServiceBase
	{
		public abstract Task InitService();
	}
}