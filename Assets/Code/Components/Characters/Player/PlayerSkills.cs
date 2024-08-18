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
		private AudioClip _eraseSound;
		private AudioClip _fireSound;
		private AudioSourcesController _audioController;
		private Camera _camera;
		private ColorHolderBase _playerColorHolder;

		private FactoryService _factoryService;
		private PlayerAnimation _playerAnimation;

		private void Awake()
		{
			_audioController = Services.AudioService.AudioSourcesController;
			_fireSound = Services.AssetService.SoundsConfig.FireClip;
			_eraseSound = Services.AssetService.SoundsConfig.EraseClip;
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

		public Cooldown FireballCooldown { get; private set; }
		public Cooldown EraserCooldown { get; private set; }
		public bool IsAttacking { get; private set; }
		public bool ErasableMode { get; private set; } = false;

		public void SwitchErasableMode()
		{
			if (CanErase())
			{
				ErasableMode = true;

				foreach (IErasable erasable in _factoryService.Erasables)
					erasable.Highlight(true);

				if (SystemInfo.deviceType == DeviceType.Desktop)
					UseEraserDesktop();
				else
					GlobalEventManager.OnScreenTap += UseEraser;

				return;
			}

			ErasableMode = false;

			foreach (IErasable erasable in _factoryService.Erasables)
				erasable.Highlight(false);

			GlobalEventManager.OnScreenTap -= UseEraser;
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
			_audioController.PlaySoundOneShot(_fireSound);
			_factoryService.CreateFireball(
				position: transform.position,
				direction: GetShootDirection(),
				rotation: Vector3.zero);

			FireballCooldown.Reset();
			IsAttacking = false;
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

			_audioController.PlaySoundOneShot(_eraseSound);
			erasable.Erase();
			SwitchErasableMode();
			EraserCooldown.Reset();
		}

		private void UseEraserDesktop()
		{
			IErasable erasable = null;
			float distance = 300;

			foreach (IErasable e in _factoryService.Erasables)
			{
				float newDistance = Vector3.Distance(transform.position, e.Position);

				Debug.Log(newDistance);

				if (distance > newDistance)
				{
					distance = newDistance;
					erasable = e;
				}
			}

			if (erasable != null)
			{
				_audioController.PlaySoundOneShot(_eraseSound);
				erasable.Erase();
				SwitchErasableMode();
				EraserCooldown.Reset();
			}
		}

		private Vector2 GetShootDirection() =>
			new(transform.localScale.x, 0);

		private bool CanLaunchFireball() =>
			_playerColorHolder.ColorToCheck == PlayerColor.Red && FireballCooldown.IsReady;

		private bool CanErase() =>
			_playerColorHolder.ColorToCheck == PlayerColor.Gray && !ErasableMode && EraserCooldown.IsReady;
	}
}