using System.Collections.Generic;
using UnityEngine;
using Utils.Constants;

namespace Configs
{
	[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/Scenes config")]
	public sealed class ScenesConfig : ScriptableObject
	{
		public List<SceneSet> sceneSets;
		
		public SceneSet GetSceneSet(SceneSets set)
		{
			foreach (SceneSet sceneSet in sceneSets)
				if (sceneSet.set == set)
					return sceneSet;

			Debug.LogError("Can't find scene set");
			return null;
		}
	}
}