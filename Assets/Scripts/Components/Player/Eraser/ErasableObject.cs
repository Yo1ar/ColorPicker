using System;
using GameCore.GameServices;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils.Constants;

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

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Colors.GreenT;
			Gizmos.DrawCube(transform.position.AddY(_eraserAboveOffset), Vector3.one / 2);
		}
	}
}