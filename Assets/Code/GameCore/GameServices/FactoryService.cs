using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Eraser;
using GameCore.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace GameCore.GameServices
{
	public sealed class FactoryService : ServiceBase
	{
		private AssetService _assetService;

		public GameObject Player { get; private set; }
		public List<IErasable> Erasables { get; } = new();

		public readonly UnityEvent OnPlayerCreated = new();

		public FactoryService(AssetService assetService)
		{
			_assetService = assetService;
			GlobalEventManager.OnLevelUnloaded.AddListener(RemovePlayer);
		}

		private void RemovePlayer() =>
			Object.DestroyImmediate(Player);

		public override Task InitService()
		{
			_assetService = Services.AssetService;

			Game.GameLogger.GameLog("Initialized", this);
			return Task.CompletedTask;
		}

		public void CreatePlayer(Vector3 position)
		{
			Player = Object.Instantiate(_assetService.Player, position, Quaternion.identity) as GameObject;
			OnPlayerCreated?.Invoke();
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