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

		public InputService() =>
			_actions = new PlayerActions();

		public override Task InitService()
		{
			_actions.Enable();

			_actions.Main.Move.started += InvokePlayerMove;
			_actions.Main.Move.performed += InvokePlayerMove;
			_actions.Main.Move.canceled += InvokePlayerMove;

			_actions.Main.Jump.started += InvokePlayerJump;

			_actions.Main.Erase.started += InvokePlayerErase;

			_actions.Main.LaunchFireball.started += InvokeLaunchFireball;

			_actions.Main.OnScreenTap.started += InvokeOnScreenTap;

			_actions.Main.Back.started += InvokeBack;

			Game.GameLogger.GameLog("Initialized", this);
			return Task.CompletedTask;
		}

		private void InvokePlayerMove(InputAction.CallbackContext obj)
		{
			var moveVector = obj.ReadValue<Vector2>();
			PlayerEventManager.OnMove?.Invoke(moveVector.x);

			if (obj.started)
				Game.GameLogger.InputLog("Move started", this);

			if (obj.canceled)
				Game.GameLogger.InputLog("Move canceled", this);
		}

		private void InvokePlayerJump(InputAction.CallbackContext obj)
		{
			PlayerEventManager.OnJump?.Invoke();
			Game.GameLogger.InputLog("Jump started", this);
		}

		private void InvokePlayerErase(InputAction.CallbackContext obj)
		{
			PlayerEventManager.OnErase?.Invoke();
			Game.GameLogger.InputLog("Erase started", this);
		}

		private void InvokeLaunchFireball(InputAction.CallbackContext obj)
		{
			PlayerEventManager.OnShoot?.Invoke();
			Game.GameLogger.InputLog("Fireball started", this);
		}

		private void InvokeOnScreenTap(InputAction.CallbackContext obj)
		{
			GlobalEventManager.OnScreenTap?.Invoke();
			Game.GameLogger.InputLog("OnScreenTap started", this);
		}

		private void InvokeBack(InputAction.CallbackContext obj)
		{
			GlobalEventManager.OnBackPressed?.Invoke();
			Game.GameLogger.InputLog("Jump " + obj.phase, this);
		}

		~InputService()
		{
			_actions.Main.Move.started -= InvokePlayerMove;
			_actions.Main.Move.performed -= InvokePlayerMove;
			_actions.Main.Move.canceled -= InvokePlayerMove;

			_actions.Main.Jump.started -= InvokePlayerJump;

			_actions.Main.Erase.started -= InvokePlayerErase;

			_actions.Main.LaunchFireball.started -= InvokeLaunchFireball;

			_actions.Main.OnScreenTap.canceled -= InvokeOnScreenTap;

			_actions.Main.Back.started -= InvokeBack;
		}
	}
}