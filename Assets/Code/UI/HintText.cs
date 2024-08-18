using TMPro;
using UnityEngine;

namespace UI
{
	public class HintText : MonoBehaviour
	{
		[SerializeField] private string _text;
		private TMP_Text _textComponent;

		private void Awake() =>
			_textComponent = GetComponentInChildren<TMP_Text>();

		private void Start() =>
			_textComponent.SetText(_text);

		private void OnValidate()
		{
			if (_textComponent == null)
				Awake();
			
			Start();
		}
	}
}