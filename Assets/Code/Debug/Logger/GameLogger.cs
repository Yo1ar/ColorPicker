using Utils.Constants;

namespace Logger
{
	public class GameLogger
	{
		private readonly LogEvent _gameLogEvent;
		private readonly LogEvent _gameLoopLogEvent;
		private readonly LogEvent _inputLogEvent;
		private readonly LogEvent _uiLogEvent;

		public GameLogger()
		{
			_gameLogEvent = new LogEvent("GameCore", EColors.Red);
			_gameLoopLogEvent = new LogEvent("Gameplay", EColors.Blue);
			_inputLogEvent = new LogEvent("Input", EColors.White);
			_uiLogEvent = new LogEvent("UI", EColors.Green);
		}

		public void SwitchAllLogActivity(bool isActive)
		{
			_gameLogEvent.IsActive = isActive;
			_gameLoopLogEvent.IsActive = isActive;
			_inputLogEvent.IsActive = isActive;
			_uiLogEvent.IsActive = isActive;
		}

		public void GameLog<T>(string message, T context) =>
			_gameLogEvent.Log(message, context);

		public void GameLoopLog<T>(string message, T context) =>
			_gameLoopLogEvent.Log(message, context);

		public void InputLog<T>(string message, T context) =>
			_inputLogEvent.Log(message, context);

		public void UILog<T>(string message, T context) =>
			_uiLogEvent.Log(message, context);
	}
}