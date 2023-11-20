using System.Threading.Tasks;
using Characters.Player;
using GameCore.GameServices;
using UnityEngine;
using Utils;

namespace Level
{
	public class RespawnPlayerBehaviour : MonoBehaviour
	{
		[SerializeField] private float _respawnTime;
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
			_playerHealth.OnDeath.AddListener(CreatePlayerDelayed);
		}

		private async void CreatePlayerDelayed()
		{
			await Task.Delay(_respawnTime.SecAsMillisec());

			_factoryService.CreatePlayer(transform.position);
			_playerHealth = _factoryService.Player.GetComponent<PlayerHealth>();
			_playerHealth.OnDeath.AddListener(CreatePlayerDelayed);
		}
	}
}
