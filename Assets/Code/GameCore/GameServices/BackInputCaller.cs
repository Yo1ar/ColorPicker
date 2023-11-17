using System;
using UnityEngine;

namespace GameCore.GameServices
{
	public class BackInputCaller : MonoBehaviour
	{
		public event Action OnBackPressed;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				OnBackPressed?.Invoke();
		}
	}
}