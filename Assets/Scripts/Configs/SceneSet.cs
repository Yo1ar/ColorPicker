using System;
using Udar.SceneManager;
using Utils.Constants;

namespace Configs
{
	[Serializable]
	public sealed class SceneSet
	{
		public SceneSets set;
		public SceneField mainScene;
		public SceneField uiScene;
	}
}