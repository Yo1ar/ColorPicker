using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Player.Eraser;
using GameCore.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace GameCore.GameServices
{
	public sealed class FactoryService : ServiceBase
	{
		private AssetService _assetService;
		private GameObject _player;

		public Transform Player => _player.transform;
		public List<IErasable> Erasables { get; } = new();

		public readonly UnityEvent<Transform> OnPlayerCreated = new();

		public FactoryService(AssetService assetService)
		{
			_assetService = assetService;
			GlobalEventManager.OnLevelUnloaded.AddListener(RemovePlayer);
		}

		private void RemovePlayer() =>
			Object.DestroyImmediate(_player);

		public override Task InitService()
		{
			_assetService = Services.AssetService;
			return Task.CompletedTask;
		}

		public void CreatePlayer(Vector3 position)
		{
			_player = Object.Instantiate(_assetService.Player, position, Quaternion.identity) as GameObject;
			OnPlayerCreated?.Invoke(_player.transform);
		}

		public void AddErasable(IErasable erasable) =>
			Erasables.Add(erasable);

		public void RemoveErasable(ErasableObject erasable) =>
			Erasables.Remove(erasable);

		public void ClearErasables() =>
			Erasables.Clear();

		~FactoryService() =>
			GlobalEventManager.OnLevelUnloaded.RemoveListener(RemovePlayer);
	}
}