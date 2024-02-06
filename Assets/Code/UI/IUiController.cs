using System;
using UnityEngine.UIElements;

namespace UI
{
	public interface IUiController : IDisposable
	{
		VisualElement RootElement { get; }
	}
}