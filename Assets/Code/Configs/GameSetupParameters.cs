using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "game_params", menuName = "Configs/Game parameters")]
	public class GameSetupParameters: ScriptableObject
	{
		public float Fps;
		public bool EnableCheats;
		public bool EnableLogger;

		public GameSetupParameters(float fps, bool enableCheats, bool enableLogger) =>
			(Fps, EnableCheats, EnableLogger) = (fps, enableCheats, enableLogger);
	}
}