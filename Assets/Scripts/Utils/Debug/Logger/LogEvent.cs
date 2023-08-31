using System.Text;
using Utils.Constants;

namespace Utils.Debug
{
	public class LogEvent
	{
		private readonly string _eventType;
		public bool IsActive;

		public LogEvent(string eventType, EColors color)
		{
			#if UNITY_EDITOR
			_eventType = eventType.LogBold().LogColored(Colors.GetLogColor(color));
			#endif
		}

		public void Log<T>(string message, T context)
		{
			#if UNITY_EDITOR

			if (!IsActive)
				return;

			string completeMessage = BuildMessage(message, context);

			UnityEngine.Debug.Log(completeMessage);
			#endif
		}

		private string BuildMessage<T>(string message, T context) =>
			new StringBuilder()
				.Append(_eventType)
				.Append(": ")
				.Append(context.ToString().LogItalic())
				.Append("__")
				.Append(message.LogBold().LogItalic())
				.ToString();
	}
}