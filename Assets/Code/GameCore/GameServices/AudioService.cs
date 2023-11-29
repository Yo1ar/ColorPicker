using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace GameCore.GameServices
{
	public class AudioService : ServiceBase
	{
		private AssetService _assetService;
		private AudioMixer _mixer;
		
		public AudioSourcesController AudioSourcesController { get; private set; }
		
		public AudioService(AssetService assetService) =>
			_assetService = assetService;

		public override Task InitService()
		{
			AudioSourcesController = Object.Instantiate(_assetService.AudioSourcesController);
			return Task.CompletedTask;
		}
	}
}