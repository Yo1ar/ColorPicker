using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace GameCore.GameServices
{
	public sealed class FactoryService : ServiceBase
	{
		private readonly ProjectilePool _fireballPool;
		private readonly ProjectilePool _dropPool;
		private AssetService _assetService;

		public GameObject Player { get; private set; }
		public List<IErasable> Erasables { get; } = new();

		public readonly UnityEvent OnPlayerCreated = new();

		public FactoryService(AssetService assetService)
		{
			_assetService = assetService;
			_fireballPool = new ProjectilePool(_assetService.FireballProjectile);
			_dropPool = new ProjectilePool(_assetService.DropProjectile);
			GlobalEventManager.OnLevelUnloaded.AddListener(RemovePlayer);
		}

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

		public void CreateFireball(Vector3 position, Vector2 direction, Vector3 rotation) =>
			_fireballPool.LaunchProjectile(position, direction, rotation);

		public void CreateDrop(Vector3 position, Vector3 direction, Vector3 rotation) =>
			_dropPool.LaunchProjectile(position, direction, rotation);

		public void AddErasable(IErasable erasable) =>
			Erasables.Add(erasable);

		public void RemoveErasable(ErasableObject erasable) =>
			Erasables.Remove(erasable);

		public void ClearErasables() =>
			Erasables.Clear();

		private void RemovePlayer() =>
			Object.DestroyImmediate(Player);

		~FactoryService() =>
			GlobalEventManager.OnLevelUnloaded.RemoveListener(RemovePlayer);
	}
}