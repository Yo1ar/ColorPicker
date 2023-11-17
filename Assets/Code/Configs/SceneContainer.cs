using System;
using Udar.SceneManager;
using Utils.Constants;

namespace Configs
{
	[Serializable]
	public sealed class SceneContainer
	{
		public SceneSets Set;
		public SceneField MainScene;
		public SceneField UIScene;
	}
}