using Cinemachine;
using GameCore.GameServices;
using UnityEngine;

namespace Components.Level
{
	public class CinemachineSetup : MonoBehaviour
	{
		private CinemachineVirtualCamera _cmCamera;
		private FactoryService _factoryService;

		private void Awake()
		{
			_cmCamera = GetComponent<CinemachineVirtualCamera>();
			_factoryService = Services.FactoryService;

			if (_factoryService.Player != null)
				InitPlayer();
			else
				_factoryService.OnPlayerCreated.AddListener(InitPlayer);
		}

		private void OnDisable() => 
			Services.FactoryService.OnPlayerCreated.RemoveListener(InitPlayer);

		private void InitPlayer()
		{
			_cmCamera.Follow = _factoryService.Player.transform;
			_factoryService.Player.transform.parent = _cmCamera.transform.parent;
		}
	}
}
