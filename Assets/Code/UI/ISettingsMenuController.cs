using System;
using UnityEngine.UIElements;

namespace UI
{
	public interface ISettingsMenuController : IUiController
	{
		Button BackButton { get; }
	}
}