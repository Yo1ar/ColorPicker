using TMPro;
using UnityEngine;

namespace Components.Enemies
{
	public class HealthLabel : MonoBehaviour
	{
		private TMP_Text _text;

		private void Awake() =>
			_text = GetComponentInChildren<TMP_Text>();

		public void SetLives(int lives) =>
			_text.SetText(lives.ToString());
	}
}