using System;
using GameCore.GameServices;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils.Constants;
using Utils.Debug;

namespace Components.Player.Eraser
{
	public class ErasableObject : MonoBehaviour, IErasable
	{
		[SerializeField] private GameObject _eraserAbove;
		[SerializeField] private float _eraserAboveOffset;

		private void Awake()
		{
			_eraserAbove = Instantiate(_eraserAbove, transform.position.AddY(_eraserAboveOffset), Quaternion.identity);
			_eraserAbove.transform.SetParent(transform);
			Highlight(false);
		}

		private void Start() =>
			Services.FactoryService.AddToErasable(this);

		public void Erase() =>
			gameObject.SetActive(false);

		public void Highlight(bool value) =>
			_eraserAbove.SetActive(value);

		private void OnDrawGizmosSelected() =>
			SceneViewLabels.DrawHandlesLabel(transform.position + Vector3.up * 2, "Erasable", Colors.Red);
	}
}