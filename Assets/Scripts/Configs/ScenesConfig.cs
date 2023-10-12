using System.Collections.Generic;
using UnityEngine;
using Utils.Constants;

namespace Configs
{
	[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/Scenes config")]
	public sealed class ScenesConfig : ScriptableObject
	{
		public List<SceneContainer> SceneContainers;
		
		public SceneContainer GetSceneContainer(SceneSets set)
		{
			foreach (SceneContainer container in SceneContainers)
				if (container.Set == set)
					return container;

			UnityEngine.Debug.LogError("Can't find scene set");
			return null;
		}
	}
}