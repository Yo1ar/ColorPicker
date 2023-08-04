using UnityEngine;

namespace Utils.Debug
{
	public static class Logger
	{
		private const string ColorPrefixStringTemplate = "<{0}>{1}</color>";
		
		public static void LogEvent(string message, Object context)
		{
			string prefix = string.Format(ColorPrefixStringTemplate, "color=cyan", "Event: ");
			UnityEngine.Debug.Log(prefix + message, context);
		}
	}
}