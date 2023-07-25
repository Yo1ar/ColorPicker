using System;
using System.Threading.Tasks;
using Configs;
using GameCore.GameUI;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Constants;

namespace GameCore
{
	public sealed class SceneLoader
	{
		private SceneContainer _currSceneContainer;
		private readonly LoadingScreen _loadingScreen;
		private AsyncOperation _mainSceneLoading;

		public SceneLoader(LoadingScreen loadingScreen) =>
			_loadingScreen = loadingScreen;

		public async Task LoadSceneSet(SceneContainer newSceneContainer, Action onLoadMainScene)
		{
			_loadingScreen.Show();

			if (_currSceneContainer != null)
				await UnloadCurrentSceneSet();

			_currSceneContainer = newSceneContainer;
			if (newSceneContainer.Set != SceneSets.MainMenu)
				LoadMainScene(_currSceneContainer.MainScene, onLoadMainScene);

			await LoadUI(_currSceneContainer.UIScene);

			_loadingScreen.Hide();
		}

		private async void LoadMainScene(SceneField scene, Action onLoaded)
		{
			_mainSceneLoading = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);

			while (!_mainSceneLoading.isDone)
				await Task.Yield();
			
			onLoaded?.Invoke();
		}

		private async Task LoadUI(SceneField scene)
		{
			AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
			while (!sceneLoad.isDone)
				await Task.Yield();
		}

		private void RunLoadedScene()
		{
			_mainSceneLoading.allowSceneActivation = true;
			_loadingScreen.Hide();
		}

		private async Task UnloadCurrentSceneSet()
		{
			if (_currSceneContainer.Set != SceneSets.MainMenu)
			{
				AsyncOperation sceneUnload1 = SceneManager.UnloadSceneAsync(_currSceneContainer.MainScene.Name);
				while (sceneUnload1.isDone)
					await Task.Yield();
			}

			AsyncOperation sceneUnload2 = SceneManager.UnloadSceneAsync(_currSceneContainer.UIScene.Name);
			while (sceneUnload2.isDone)
				await Task.Yield();
		}
	}
}