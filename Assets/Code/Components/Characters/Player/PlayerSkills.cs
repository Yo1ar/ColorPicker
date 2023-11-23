using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Characters.Player
{
	public class PlayerSkills : MonoBehaviour, IPlayerSkills
	{
		[SerializeField] private float _fireballShootRecharge = 1f;
		[SerializeField] private float _eraserRecharge = 5;
		
		private FactoryService _factoryService;
		private PlayerAnimation _playerAnimation;
		private ColorHolderBase _playerColorHolder;
		private Camera _camera;

		public Cooldown FireballCooldown { get; private set; }
		public Cooldown EraserCooldown { get; private set; }
		public bool IsAttacking { get; private set; }
		public bool ErasableMode { get; private set; } = false;

		private void Awake()
		{
			_factoryService = Services.FactoryService;
			_playerAnimation = GetComponent<PlayerAnimation>();
			_playerColorHolder = GetComponent<ColorHolderBase>();
			_camera = Camera.main;
			FireballCooldown = new Cooldown(_fireballShootRecharge);
			EraserCooldown = new Cooldown(_eraserRecharge);
		}

		private void OnEnable()
		{
			PlayerEventManager.OnShoot.AddListener(ShootFireball);
			PlayerEventManager.OnErase.AddListener(SwitchErasableMode);
		}

		private void OnDisable()
		{
			PlayerEventManager.OnShoot.RemoveListener(ShootFireball);
			PlayerEventManager.OnErase.RemoveListener(SwitchErasableMode);
		}

		public void SwitchErasableMode()
		{
			if (CanErase())
			{
				ErasableMode = true;

				foreach (IErasable erasable in _factoryService.Erasables)
					erasable.Highlight(true);

				GlobalEventManager.OnScreenTap.AddListener(UseEraser);

				return;
			}

			ErasableMode = false;

			foreach (IErasable erasable in _factoryService.Erasables)
				erasable.Highlight(false);

			GlobalEventManager.OnScreenTap.RemoveListener(UseEraser);
		}

		private void UseEraser()
		{
			Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit =
				Physics2D.Raycast(mouseWorldPos,
					Vector3.forward,
					100f,
					LayerMask.GetMask("Erasable"));

			if (!hit || !hit.transform.TryGetComponent(out IErasable erasable))
				return;

			erasable.Erase();
			SwitchErasableMode();
			EraserCooldown.Reset();
		}

		public void ShootFireball()
		{
			if (!CanLaunchFireball())
				return;

			IsAttacking = true;

			_playerAnimation.AnimatePreAttack();
		}

		public void LaunchFireball()
		{
			_factoryService.CreateFireball(
				position: transform.position,
				direction: GetShootDirection(),
				rotation: Vector3.zero);

			FireballCooldown.Reset();
			IsAttacking = false;
		}

		private Vector2 GetShootDirection() =>
			new(transform.localScale.x, 0);
		
		private bool CanLaunchFireball() =>
			_playerColorHolder.ColorToCheck == EColors.Red && FireballCooldown.IsReady;
		
		private bool CanErase() =>
			_playerColorHolder.ColorToCheck == EColors.Gray && !ErasableMode && EraserCooldown.IsReady;
	}
}