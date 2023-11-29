using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheatWindow : MonoBehaviour
{
	[SerializeField] private CheatElement _prefab;
	[SerializeField] private Transform _content;
	[SerializeField] private Button _handle;

	private bool _isOpen = false;

	private void Awake() =>
		DontDestroyOnLoad(this.transform.parent);

	private void OnEnable() =>
		_handle.onClick.AddListener(ShowHideWindow);

	private void OnDisable() =>
		_handle.onClick.RemoveAllListeners();

	public void CreateToggle(string cheatName, bool initialValue, UnityAction<bool> onValueChanged)
	{
		CheatElement element = Instantiate(_prefab, _content);
		element.AddToggle(cheatName, initialValue, onValueChanged);
	}
		
		
	public void CreateInputInt(string cheatName, int initialValue, UnityAction<string> onValueChanged)
	{
		CheatElement element = Instantiate(_prefab, _content);
		element.AddInputFieldInt(cheatName, initialValue, onValueChanged);
	}
		
	public void CreateInputFloat(string cheatName, float initialValue, UnityAction<string> onValueChanged)
	{
		CheatElement element = Instantiate(_prefab, _content);
		element.AddInputFieldFloat(cheatName, initialValue, onValueChanged);
	}


	private void ShowHideWindow()
	{
		var rectTransform = transform as RectTransform;
			
		rectTransform.anchoredPosition = _isOpen
			? new Vector2(-rectTransform.rect.width, 0)
			: Vector2.zero;

		_isOpen = !_isOpen;
	}
}