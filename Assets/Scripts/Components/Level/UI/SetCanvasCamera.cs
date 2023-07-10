using UnityEngine;

namespace Components.Level.UI
{
	public class SetCanvasCamera : MonoBehaviour
	{
		private void Awake()
		{
			var canvas = GetComponent<Canvas>();
			canvas.worldCamera = Camera.main;
		}
	}
}