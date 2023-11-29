using Characters.Player;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine;
using Utils.Constants;

namespace Level
{
	public class RespawnPlayerBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneSets _reloadScene;
		private FactoryService _factoryService;
		private PlayerHealth _playerHealth;

		private void Awake() =>
			_factoryService = Services.FactoryService;

		private void Start() =>
			CreatePlayer();

		private void CreatePlayer()
		{
			_factoryService.CreatePlayer(transform.position);
			_playerHealth = _factoryService.Player.GetComponent<PlayerHealth>();
			_playerHealth.OnDeath.AddListener(ReloadLevel);
		}

		private void ReloadLevel() =>
			GameStateMachine.Instance.EnterLoadLevelState(_reloadScene);
	}
}
