using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Characters.Player
{
	public class PlayerController : MonoBehaviour, IWildColorContainer, IPlayerSkills
	{
		[SerializeField] private float _fireballShootRecharge = 1f;
		
		private FactoryService _factoryService;
		private PlayerMove _playerMove;
		private PlayerJump _playerJump;
		private PlayerAnimation _playerAnimation;
		private ColorHolderBase _playerColorHolder;
		private Camera _camera;

		public Cooldown FireballCooldown { get; private set; }
		public bool ErasableMode { get; private set; } = false;
		public int WildColorBonus { get; private set; } = 5;
		public bool IsAttacking { get; set; }

		private void Awake()
		{
			_factoryService = Services.FactoryService;
			_playerMove = GetComponent<PlayerMove>();
			_playerJump = GetComponent<PlayerJump>();
			_playerAnimation = GetComponent<PlayerAnimation>();
			_playerColorHolder = GetComponent<ColorHolderBase>();
			_camera = Camera.main;
			FireballCooldown = new Cooldown(_fireballShootRecharge);
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

		public void SetWldColorBonus(int value) =>
			WildColorBonus = value;

		public void AddWildColorBonus() =>
			WildColorBonus++;

		public bool TrySpendWildColorBonus()
		{
			if (WildColorBonus - 1 < 0)
				return false;

			WildColorBonus--;
			return true;
		}

		public bool CanUseWildColorBonus() =>
			WildColorBonus >= 1;

		public void SwitchErasableMode()
		{
			if (_playerColorHolder.ColorToCheck == EColors.White && !ErasableMode)
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
					LayerMask.GetMask("Enemy"));

			if (!hit || hit.transform.TryGetComponent(out IErasable erasable))
				return;

			erasable.Erase();
			SwitchErasableMode();
		}

		public void ShootFireball()
		{
			if (!CanLaunchFireball())
				return;

			IsAttacking = true;

			_playerAnimation.AnimatePreAttack();
			
			bool CanLaunchFireball()
			{
				return _playerColorHolder.ColorToCheck == EColors.Red
				       && FireballCooldown.IsReady;
			}
		}

		public void LaunchFireball()
		{
			_factoryService.CreateFireball(
				position: transform.position,
				direction: GetShootDirection(),
				rotation: Vector3.zero);

			FireballCooldown.Reset();
			IsAttacking = false;
			
			Vector2 GetShootDirection() =>
				new(transform.localScale.x, 0);
		}
	}
}