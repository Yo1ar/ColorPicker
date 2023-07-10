using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.GameServices
{
	public sealed class FactoryService : ServiceBase
	{
		private AssetService _assetService;
		private GameObject _player;
		public event Action<Transform> OnPlayerCreated;
		public Transform Player => _player.transform;

		public override Task InitService()
		{
			_assetService = ServiceLocator.assetService;
			return Task.CompletedTask;
		}

		public void CreatePlayer(Vector3 position)
		{
			_player = Object.Instantiate(_assetService.Player, position, Quaternion.identity) as GameObject;
			OnPlayerCreated?.Invoke(_player.transform);
		}
	}
}