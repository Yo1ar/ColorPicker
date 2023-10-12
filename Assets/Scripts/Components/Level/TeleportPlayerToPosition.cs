using System.Threading.Tasks;
using Cinemachine;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;

namespace Components.Level
{
	public class TeleportPlayerToPosition : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _target;
		private readonly float _waitPlayerDisappear = 0.5f;
		private readonly float _waitCameraFollow = 0.5f;
		private readonly float _waitPlayerAppear = 0.2f;
		private readonly float _waitPortalDisappears = 0.2f;

		private Rigidbody2D _playerRigidbody;
		private SortingGroup _playerSortingGroup;
		private CinemachineVirtualCamera _cmCamera;
		private FactoryService _factoryService;

		private void Awake()
		{
			_cmCamera = FindObjectOfType<CinemachineVirtualCamera>();
			_factoryService = Services.FactoryService;

			if (_factoryService.Player != null)
				InitPLayer();
			else
				_factoryService.OnPlayerCreated.AddListener(InitPLayer);
		}

		private void OnDisable() =>
			Services.FactoryService.OnPlayerCreated.RemoveListener(InitPLayer);

		private void InitPLayer()
		{
			_factoryService.Player.TryGetComponent(out _playerRigidbody);
			_factoryService.Player.TryGetComponent(out _playerSortingGroup);
		}

		public async void Teleport(Transform player)
		{
			HidePlayer();
			SwitchTargetRenderer(true);
			
			await Task.Delay(_waitPlayerDisappear.AsMilliseconds());
			
			_cmCamera.Follow = _target.transform;
			
			await Task.Delay(_waitCameraFollow.AsMilliseconds());
			
			_playerRigidbody.MovePosition(_target.transform.position);
			
			await Task.Delay(_waitPlayerAppear.AsMilliseconds());
			
			ShowPlayer();
			ReturnCameraToNormalFollow();

			await Task.Delay(_waitPortalDisappears.AsMilliseconds());

			SwitchTargetRenderer(false);
		}

		private void SwitchTargetRenderer(bool value) =>
			_target.enabled = value;

		private void ReturnCameraToNormalFollow()
		{
			_playerRigidbody.velocity = Vector2.zero;
			_cmCamera.Follow = _playerRigidbody.transform;
		}

		private void ShowPlayer()
		{
			_playerSortingGroup.sortingLayerName = "Player";
			_playerSortingGroup.sortingOrder = 0;
			_playerRigidbody.gravityScale = 1;
			_playerRigidbody.velocity = Vector2.zero;
		}

		private void HidePlayer()
		{
			_playerSortingGroup.sortingLayerName = "Background";
			_playerSortingGroup.sortingOrder = -1;
			_playerRigidbody.gravityScale = 0;
			_playerRigidbody.velocity = Vector2.zero;
		}
	}
}