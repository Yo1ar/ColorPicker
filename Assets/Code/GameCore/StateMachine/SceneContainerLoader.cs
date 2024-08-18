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
		private const float LoadLimit = 0.9f;
		private readonly SceneContainer _sceneContainer;
		private AsyncOperation _mainLoading;
		private AsyncOperation _uiLoading;

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
			GlobalEventManager.OnLevelLoaded -= Activate;

			_mainLoading.allowSceneActivation = true;
		}

		private async Task LoadSceneSet()
		{
			Debug.Log("2. loading in process");

			while (_mainLoading.progress <= LoadLimit
			       && _uiLoading.progress <= LoadLimit)
				await Task.Yield();

			GlobalEventManager.OnLevelLoaded += Activate;
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