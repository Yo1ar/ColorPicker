using System.Threading.Tasks;
using Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public class InputService : ServiceBase
	{
		private FactoryService _factoryService;
		private readonly PlayerActions _actions;
		private readonly PlayerEvents _playerEvents;
		private readonly GameEvents _gameEvents;

		public InputService(PlayerEvents playerEvents, GameEvents gameEvents)
		{
			_actions = new PlayerActions();
			_playerEvents = playerEvents;
			_gameEvents = gameEvents;
		}

		public override Task InitService()
		{
			_actions.Enable();

			_actions.Main.Move.performed += InvokePlayerMove;
			_actions.Main.Move.canceled += InvokePlayerMove;
			_actions.Main.Jump.started += InvokePlayerJump;
			_actions.Main.Erase.started += InvokePlayerErase;
			_actions.Main.LaunchFireball.started += InvokeLaunchFireball;
			_actions.Main.OnScreenTap.started += InvokeOnScreenTap;
			_actions.Main.Back.canceled += InvokeBack;

			return Task.CompletedTask;
		}

		private void InvokePlayerMove(InputAction.CallbackContext obj)
		{
			var moveVector = obj.ReadValue<Vector2>();
			_playerEvents.OnMove?.Invoke(moveVector.x);
		}

		private void InvokePlayerJump(InputAction.CallbackContext obj) =>
			_playerEvents.OnJump?.Invoke();

		private void InvokePlayerErase(InputAction.CallbackContext obj) =>
			_playerEvents.OnErase?.Invoke();

		private void InvokeLaunchFireball(InputAction.CallbackContext obj) =>
			_playerEvents.OnShoot?.Invoke();

		private void InvokeOnScreenTap(InputAction.CallbackContext obj) =>
			_gameEvents.OnScreenTap?.Invoke();

		private void InvokeBack(InputAction.CallbackContext obj) =>
			_gameEvents.OnBackPressed?.Invoke();

		~InputService()
		{
			_actions.Main.Move.performed -= InvokePlayerMove;
			_actions.Main.Move.canceled -= InvokePlayerMove;
			_actions.Main.Jump.started -= InvokePlayerJump;
			_actions.Main.Erase.started -= InvokePlayerErase;
			_actions.Main.LaunchFireball.started -= InvokeLaunchFireball;
			_actions.Main.OnScreenTap.canceled -= InvokeOnScreenTap;
			_actions.Main.Back.canceled -= InvokeBack;
		}
	}
}