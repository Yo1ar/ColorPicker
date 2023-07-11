using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Player.Eraser;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.GameServices
{
	public sealed class FactoryService : ServiceBase
	{
		private AssetService _assetService;
		private GameObject _player;

		public Transform Player => _player.transform;
		public List<IErasable> Erasables { get; } = new();

		public event Action<Transform> OnPlayerCreated;

		public override Task InitService()
		{
			_assetService = ServiceLocator.assetService;
			return Task.CompletedTask;
		}
		
		public void AddToErasable(IErasable erasable) =>
			Erasables.Add(erasable);

		public void CreatePlayer(Vector3 position)
		{
			_player = Object.Instantiate(_assetService.Player, position, Quaternion.identity) as GameObject;
			OnPlayerCreated?.Invoke(_player.transform);
		}
	}
}