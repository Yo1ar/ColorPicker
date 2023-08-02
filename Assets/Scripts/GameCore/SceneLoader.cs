using System;
using System.Threading.Tasks;
using Configs;
using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Constants;

namespace GameCore
{
	public sealed class SceneLoader
	{
		private readonly LoadingScreen _loadingScreen;
		private readonly ConfigService _configService;
		private SceneContainer _currSceneContainer;
		private AsyncOperation _mainSceneLoading;
		private AsyncOperation _uiSceneLoading;
		private bool _mainMenuLoaded;

		public SceneLoader(LoadingScreen loadingScreen)
		{
			_configService = Services.ConfigService;
			_loadingScreen = loadingScreen;
		}

		public async Task LoadMainMenu()
		{
			_loadingScreen.Show();

			if (_currSceneContainer != null)
				await UnloadCurrentSceneSet();

			AsyncOperation loading = LoadSceneAsyncAdditive(Scenes.MainMenuUI);
			loading.completed += HideLoadingScreen;

			while (loading.progress < 0.9f || !_loadingScreen.IsShown)
				await Task.Yield();

			loading.allowSceneActivation = true;
			_mainMenuLoaded = true;
		}

		public async Task LoadSceneSet(SceneContainer newSceneContainer, Action<AsyncOperation> onLoadMainScene)
		{
			_loadingScreen.Show();

			if (_currSceneContainer != null)
				await UnloadCurrentSceneSet();

			if (_mainMenuLoaded)
				await UnloadMainMenu();

			_currSceneContainer = newSceneContainer;

			_mainSceneLoading = LoadSceneAsyncAdditive(_currSceneContainer.MainScene.Name);
			_uiSceneLoading = LoadSceneAsyncAdditive(_currSceneContainer.UIScene.Name);

			while ((_mainSceneLoading.progress < 0.9f && _uiSceneLoading.progress < 0.9f) || !_loadingScreen.IsShown)
				await Task.Yield();

			_loadingScreen.SetTapOnTheScreenText();
			
			_mainSceneLoading.completed += FinishUILoading;
			_mainSceneLoading.completed += onLoadMainScene.Invoke;
			_configService.GameEvents.OnScreenTap += FinishLevelLoading;
		}

		private void FinishUILoading(AsyncOperation obj) =>
			_uiSceneLoading.allowSceneActivation = true;

		private void FinishLevelLoading()
		{
			_mainSceneLoading.allowSceneActivation = true;
			_configService.GameEvents.OnScreenTap -= FinishLevelLoading;
			HideLoadingScreen();
		}

		private void HideLoadingScreen(AsyncOperation asyncOperation = null)
		{
			if (_loadingScreen.IsShown)
				_loadingScreen.Hide();
			else
				_loadingScreen.OnShown += HideLoadingOnShown;
		}

		private void HideLoadingOnShown()
		{
			_loadingScreen.OnShown -= HideLoadingOnShown;
			_loadingScreen.Hide();
		}

		private AsyncOperation LoadSceneAsyncAdditive(string scene)
		{	
			AsyncOperation loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			loading.allowSceneActivation = false;
			return loading;
		}
		
		private async Task UnloadMainMenu()
		{
			await UnloadSceneAsync(Scenes.MainMenuUI);
			_mainMenuLoaded = false;
		}

		private async Task UnloadCurrentSceneSet()
		{
			await UnloadSceneAsync(_currSceneContainer.MainScene.Name);
			await UnloadSceneAsync(_currSceneContainer.UIScene.Name);
			_currSceneContainer = null;
		}

		private async Task UnloadSceneAsync(string sceneName)
		{
			AsyncOperation unloading = SceneManager.UnloadSceneAsync(sceneName);

			while (!unloading.isDone)
				await Task.Yield();
		}
	}
}