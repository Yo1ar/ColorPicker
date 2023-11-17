using System;
using System.Threading.Tasks;
using Configs;
using GameCore.Events;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.StateMachine
{
	public class SceneContainerLoader
	{
		private readonly SceneContainer _sceneContainer;
		private AsyncOperation _mainLoading;
		private AsyncOperation _uiLoading;
		private const float LOAD_LIMIT = 0.9f;

		public SceneContainerLoader(SceneContainer container) =>
			_sceneContainer = container;

		public async Task Start(Action<AsyncOperation> onLoaded)
		{
			_mainLoading = LoadSceneAsync(_sceneContainer.MainScene);
			_uiLoading = LoadSceneAsync(_sceneContainer.UIScene);

			_mainLoading.completed += ContinueUILoading;
			_uiLoading.completed += onLoaded;
			_uiLoading.completed += InvokeOnLevelLoaded;

			await LoadSceneSet();
		}

		public void Activate()
		{
			GlobalEventManager.OnScreenTap.RemoveListener(Activate);
			
			_mainLoading.allowSceneActivation = true;
		}

		private async Task LoadSceneSet()
		{
			UnityEngine.Debug.Log("2. loading in process");
			
			while (_mainLoading.progress <= LOAD_LIMIT
			       && _uiLoading.progress <= LOAD_LIMIT)
				await Task.Yield();

			GlobalEventManager.OnLevelLoaded.AddListener(Activate);
		}

		private AsyncOperation LoadSceneAsync(SceneField scene)
		{
			AsyncOperation loading = SceneManager.LoadSceneAsync(scene.Name);
			loading.allowSceneActivation = false;
			return loading;
		}

		private void InvokeOnLevelLoaded(AsyncOperation loading) =>
			GlobalEventManager.OnLevelLoaded?.Invoke();

		private void ContinueUILoading(AsyncOperation mainLoading) =>
			_uiLoading.allowSceneActivation = true;
	}
}