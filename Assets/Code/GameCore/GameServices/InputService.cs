using System.Threading.Tasks;
using GameCore.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public class InputService : ServiceBase
	{
		private FactoryService _factoryService;
		private readonly PlayerActions _actions = new();
		private readonly BackInputCaller _backInputCaller = Object.FindObjectOfType<BackInputCaller>();

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

			_actions.Main.PickColor.started += InvokePickColor;
			_actions.Main.GrayColor.started += InvokeGrayColor;
			_actions.Main.RedColor.started += InvokeRedColor;
			_actions.Main.GreenColor.started += InvokeGreenColor;
			_actions.Main.BlueColor.started += InvokeBlueColor;

			_actions.Main.OnScreenTap.started += InvokeOnScreenTap;
			_backInputCaller.OnBackPressed += InvokeBack;

			return Task.CompletedTask;
		}

		private void InvokePickColor(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			GlobalEventManager.OnColorPick?.Invoke();
		}

		private void InvokeGrayColor(InputAction.CallbackContext obj) =>
			GlobalEventManager.OnGrayColor?.Invoke();

		private void InvokeRedColor(InputAction.CallbackContext obj) =>
			GlobalEventManager.OnRedColor?.Invoke();

		private void InvokeGreenColor(InputAction.CallbackContext obj) =>
			GlobalEventManager.OnGreenColor?.Invoke();

		private void InvokeBlueColor(InputAction.CallbackContext obj) =>
			GlobalEventManager.OnBlueColor?.Invoke();

		private void InvokePlayerMove(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			var moveVector = obj.ReadValue<Vector2>();
			PlayerEventManager.OnMove?.Invoke(moveVector.x);
		}

		private void InvokePlayerJump(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			PlayerEventManager.OnJump?.Invoke();
		}

		private void InvokePlayerSpeedUp(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			PlayerEventManager.OnSpeedUp?.Invoke();
		}

		private void InvokePlayerErase(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			PlayerEventManager.OnErase?.Invoke();
		}

		private void InvokeLaunchFireball(InputAction.CallbackContext obj)
		{
			if (Game.IsPaused)
				return;

			PlayerEventManager.OnShoot?.Invoke();
		}

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
			_actions.Main.SpeedUp.started -= InvokePlayerSpeedUp;
			_actions.Main.Erase.started -= InvokePlayerErase;
			_actions.Main.LaunchFireball.started -= InvokeLaunchFireball;

			_actions.Main.PickColor.started -= InvokePickColor;
			_actions.Main.GrayColor.started -= InvokeGrayColor;
			_actions.Main.RedColor.started -= InvokeRedColor;
			_actions.Main.GreenColor.started -= InvokeGreenColor;
			_actions.Main.BlueColor.started -= InvokeBlueColor;

			_actions.Main.OnScreenTap.canceled -= InvokeOnScreenTap;
			_backInputCaller.OnBackPressed -= InvokeBack;
		}
	}
}