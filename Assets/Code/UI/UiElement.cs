using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	[RequireComponent(typeof(UIDocument))]
	public class UiElement : MonoBehaviour
	{
		protected UIDocument Document;

		protected virtual void Awake() =>
			Document = GetComponent<UIDocument>();
	}
}