using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.UI
{
	public class LevelUIButtonCounter : MonoBehaviour
	{
		private Image _counterBg;
		private TMP_Text _counterText;

		private void Awake()
		{
			_counterBg = GetComponent<Image>();
			_counterText = GetComponentInChildren<TMP_Text>();
		}

		public void SetValue(int value) =>
			_counterText.SetText(value.ToString());

		public void TurnOff()
		{
			_counterBg.enabled = false;
			_counterText.enabled = false;
		}

		public void TurnOn()
		{
			_counterBg.enabled = true;
			_counterText.enabled = true;
		}
	}
}