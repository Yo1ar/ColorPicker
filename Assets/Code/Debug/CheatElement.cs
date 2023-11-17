using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheatElement : MonoBehaviour
{
	[SerializeField] private TMP_Text _label;
	[SerializeField] private Toggle _toggle;
	[SerializeField] private TMP_InputField _inputFieldInt;
	[SerializeField] private TMP_InputField _inputFieldFloat;

	private TMP_Text _cheatName;

	public void AddToggle(string cheatName, bool initialValue, UnityAction<bool> action)
	{
		_label.SetText(cheatName);
			
		_toggle.gameObject.SetActive(true);
		_toggle.isOn = initialValue;
			
		_toggle.onValueChanged.AddListener(action);
	}

	public void AddInputFieldInt(string cheatName, int initialValue, UnityAction<string> action)
	{
		_label.SetText(cheatName);
			
		_inputFieldInt.gameObject.SetActive(true);
		_inputFieldInt.text = initialValue.ToString();

		_inputFieldInt.onValueChanged.AddListener(action);
	}

	public void AddInputFieldFloat(string cheatName, float initialValue, UnityAction<string> action)
	{
		_label.SetText(cheatName);
			
		_inputFieldFloat.gameObject.SetActive(true);
		_inputFieldFloat.text = initialValue.ToString();
			
		_inputFieldFloat.onValueChanged.AddListener(action);
	}
}