using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public class InputService : ServiceBase
	{
		private FactoryService _factoryService;
		private PlayerActions _actions;

		public event Action<float> OnMovePressed;
		public event Action OnJumpPressed;
		public event Action OnLaunchFireballPressed;
		public event Action OnErasePressed;
		public event Action OnBackPressed;
		public event Action OnScreenTapPressed;

		public InputService() =>
			_actions = new PlayerActions();

		public override Task InitService()
		{
			_actions.Enable();

			_actions.Main.Move.performed += InvokeMove;
			_actions.Main.Move.canceled += InvokeMove;
			_actions.Main.Jump.started += InvokeJump;
			_actions.Main.Erase.started += InvokeErase;
			_actions.Main.LaunchFireball.started += InvokeLaunchFireball;
			_actions.Main.OnScreenTap.canceled += InvokeOnScreenTap;
			_actions.Main.Back.canceled += InvokeBack;
			
			return Task.CompletedTask;
		}

		private void InvokeMove(InputAction.CallbackContext obj)
		{
			var moveVector = obj.ReadValue<Vector2>();
			OnMovePressed?.Invoke(moveVector.x);
		}

		private void InvokeJump(InputAction.CallbackContext obj) =>
			OnJumpPressed?.Invoke();

		private void InvokeErase(InputAction.CallbackContext obj) =>
			OnErasePressed?.Invoke();

		private void InvokeLaunchFireball(InputAction.CallbackContext obj) =>
			OnLaunchFireballPressed?.Invoke();

		private void InvokeOnScreenTap(InputAction.CallbackContext obj) =>
			OnScreenTapPressed?.Invoke();

		private void InvokeBack(InputAction.CallbackContext obj) =>
			OnBackPressed?.Invoke();

		~InputService()
		{
			_actions.Main.Jump.performed -= InvokeJump;
			_actions.Main.Erase.performed -= InvokeErase;
			_actions.Main.LaunchFireball.performed -= InvokeLaunchFireball;
			_actions.Main.OnScreenTap.performed -= InvokeOnScreenTap;
		}
	}
}