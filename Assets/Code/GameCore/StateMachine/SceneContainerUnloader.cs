using System.Threading.Tasks;
using Configs;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.StateMachine
{
	public class SceneContainerUnloader
	{
		private readonly SceneContainer _sceneContainer;
		private AsyncOperation _mainUnloading;
		private AsyncOperation _uiUnloading;
		
		public SceneContainerUnloader(SceneContainer container) =>
			_sceneContainer = container;
		
		public async Task Start()
		{
			if (_sceneContainer == null)
			{
				await Task.CompletedTask;
				return;
			}
			
			_mainUnloading = UnloadSceneAsync(_sceneContainer.MainScene);
			_uiUnloading = UnloadSceneAsync(_sceneContainer.UIScene);
			
			await UnloadSceneSet();
		}

		private async Task UnloadSceneSet()
		{
			Debug.Log("2. unloading in process");
			
			while (!_mainUnloading.isDone
			       && _uiUnloading.isDone)
				await Task.Yield();
		}

		private AsyncOperation UnloadSceneAsync(SceneField scene) =>
			SceneManager.UnloadSceneAsync(scene.Name);
	}
}