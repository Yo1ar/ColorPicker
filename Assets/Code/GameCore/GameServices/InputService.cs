using System.Threading.Tasks;
using GameCore.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public class InputService : ServiceBase
	{
		private FactoryService _factoryService;
		private readonly PlayerActions _actions;
		private readonly BackInputCaller _backInputCaller;
		
		public InputService()
		{
			_actions = new PlayerActions();
			_backInputCaller = Object.FindObjectOfType<BackInputCaller>();
		}

		public override Task InitService()
		{
			_actions.Enable();

			_actions.Main.Move.started += InvokePlayerMove;
			_actions.Main.Move.performed += InvokePlayerMove;
			_actions.Main.Move.canceled += InvokePlayerMove;

			_actions.Main.Jump.started += InvokePlayerJump;

			_actions.Main.SpeedUp.started += InvokePlayerSpeedUp;

			_actions.Main.Erase.started += InvokePlayerErase;

			_actions.Main.LaunchFireball.started += InvokeLaunchFireball;

			_actions.Main.OnScreenTap.started += InvokeOnScreenTap;

			_backInputCaller.OnBackPressed += InvokeBack;
			return Task.CompletedTask;
		}

		private void InvokePlayerMove(InputAction.CallbackContext obj)
		{
			var moveVector = obj.ReadValue<Vector2>();
			PlayerEventManager.OnMove?.Invoke(moveVector.x);
		}

		private void InvokePlayerJump(InputAction.CallbackContext obj) =>
			PlayerEventManager.OnJump?.Invoke();

		private void InvokePlayerSpeedUp(InputAction.CallbackContext obj) =>
			PlayerEventManager.OnSpeedUp?.Invoke();

		private void InvokePlayerErase(InputAction.CallbackContext obj) =>
			PlayerEventManager.OnErase?.Invoke();

		private void InvokeLaunchFireball(InputAction.CallbackContext obj) =>
			PlayerEventManager.OnShoot?.Invoke();

		private void InvokeOnScreenTap(InputAction.CallbackContext obj) =>
			GlobalEventManager.OnScreenTap?.Invoke();

		private void InvokeBack() =>
			GlobalEventManager.OnBackPressed?.Invoke();

		~InputService()
		{
			_actions.Main.Move.started -= InvokePlayerMove;
			_actions.Main.Move.performed -= InvokePlayerMove;
			_actions.Main.Move.canceled -= InvokePlayerMove;

			_actions.Main.Jump.started -= InvokePlayerJump;

			_actions.Main.Erase.started -= InvokePlayerErase;

			_actions.Main.LaunchFireball.started -= InvokeLaunchFireball;

			_actions.Main.OnScreenTap.canceled -= InvokeOnScreenTap;

			_backInputCaller.OnBackPressed -= InvokeBack;
		}
	}
}