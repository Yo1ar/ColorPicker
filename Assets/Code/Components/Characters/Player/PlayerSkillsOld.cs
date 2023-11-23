﻿using GameCore;
using GameCore.Events;
using GameCore.GameServices;
using UnityEngine;

namespace Characters.Player
{
	// [RequireComponent(typeof(PlayerInventory))]
	// public sealed class PlayerSkillsOld : MonoBehaviour
	// {
	// 	private Projectile _projectile;
	// 	private ProjectilePool _fireballPool;
	//
	// 	private IWildColorContainer _wildColorContainer;
	// 	private PlayerAnimation _playerAnimation;
	// 	private PlayerHealth _playerHealth;
	// 	private FactoryService _factory;
	// 	private bool _erasableMode;
	// 	private bool _erasablesHighlighted;
	// 	private Camera _camera;
	//
	// 	public bool IsAttacking { get; private set; }
	//
	// 	private void Awake()
	// 	{
	// 		_wildColorContainer = GetComponent<IWildColorContainer>();
	// 		_playerAnimation = GetComponent<PlayerAnimation>();
	// 		_playerHealth = GetComponent<PlayerHealth>();
	// 		_camera = Camera.main;
	//
	// 		_factory = Services.FactoryService;
	// 		_projectile = Services.AssetService.FireballProjectile;
	// 		_fireballPool = new ProjectilePool(_projectile);
	// 	}
	//
	// 	private void OnEnable() =>
	// 		_playerHealth.OnDamage.AddListener(SetPlayerNotAttacking);
	//
	// 	private void EraseObject()
	// 	{
	// 		if (!_erasableMode)
	// 			return;
	//
	// 		Vector3 mPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
	// 		RaycastHit2D hit =
	// 			Physics2D.Raycast(mPosition, Vector3.forward, 100f, LayerMask.GetMask("Enemy"));
	//
	// 		if (hit
	// 		    && hit.transform.TryGetComponent(out IErasable erasable)
	// 		    && TryErase())
	// 		{
	// 			erasable.Erase();
	// 			
	// 			Game.GameLogger.GameLoopLog(hit.transform.name + " erased", this);
	// 			SwitchErasableMode();
	// 		}
	// 	}
	//
	// 	public void SwitchErasableMode()
	// 	{
	// 		if (_erasableMode)
	// 		{
	// 			_erasableMode = !_erasableMode;
	// 			HighlightErasables(_erasableMode);
	// 		}
	// 		else if (false)
	// 		{
	// 			_erasableMode = !_erasableMode;
	// 			HighlightErasables(_erasableMode);
	// 		}
	// 	}
	//
	// 	private void HighlightErasables(bool value)
	// 	{
	// 		foreach (IErasable erasable in _factory.Erasables)
	// 			erasable.Highlight(value);
	//
	// 		if (_erasableMode)
	// 		{
	// 			GlobalEventManager.OnScreenTap.AddListener(EraseObject);
	// 			Game.GameLogger.GameLoopLog("Erasable mode enabled", this);
	// 		}
	// 		else
	// 		{
	// 			GlobalEventManager.OnScreenTap.RemoveListener(EraseObject);
	// 			Game.GameLogger.GameLoopLog("Erasable mode disabled", this);
	// 		}
	// 	}
	//
	// 	public void TryLaunchFireball()
	// 	{
	// 		if (!CanLaunchFireball())
	// 			return;
	//
	// 		IsAttacking = true;
	//
	// 		_playerAnimation.AnimatePreAttack();
	// 	}
	//
	// 	public void LaunchFireball()
	// 	{
	// 		Game.GameLogger.GameLoopLog("Fireball launched", this);
	//
	// 		_fireballPool.LaunchProjectile(
	// 			position: transform.position,
	// 			direction: GetShootDirection(),
	// 			rotation: Vector3.zero);
	//
	// 		SetPlayerNotAttacking();
	// 	}
	//
	// 	private void SetPlayerNotAttacking() =>
	// 		IsAttacking = false;
	//
	//
	//
	// 	private bool TryErase() =>
	// 		false;
	//
	// 	private bool CanLaunchFireball() =>
	// 		false;
	// }
}