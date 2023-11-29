using System;
using Configs;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.GameUI
{
	[RequireComponent(typeof(CanvasScaler))]
	public class CanvasScalerSetup : MonoBehaviour
	{
		[SerializeField] private CanvasScalerSetupConfig _setupConfig;
	
		private CanvasScaler _canvasScaler;

		private void Awake() =>
			_canvasScaler = GetComponent<CanvasScaler>();

		private void Start() =>
			ApplyValues();

		private void Setup()
		{
			_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			_canvasScaler.referenceResolution = _setupConfig.ReferenceResolution;
			_canvasScaler.screenMatchMode = _setupConfig.ScreenMatchMode;
			_canvasScaler.matchWidthOrHeight = _setupConfig.Match;
		}

		[ContextMenu("Apply Values")]
		private void ApplyValues()
		{
			_canvasScaler = GetComponent<CanvasScaler>();
			Setup();
		}
	}
}