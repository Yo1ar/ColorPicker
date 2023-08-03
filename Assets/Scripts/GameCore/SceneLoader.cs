using System.Threading.Tasks;
using Configs;
using GameCore.Events;
using GameCore.GameUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Constants;

namespace GameCore
{
	public sealed class SceneLoader
	{
		private readonly LoadingScreen _loadingScreen;
		private SceneContainer _currSceneContainer;
		private AsyncOperation _mainSceneLoading;
		private AsyncOperation _uiSceneLoading;
		private bool _mainMenuLoaded;

		public SceneLoader(LoadingScreen loadingScreen) =>
			_loadingScreen = loadingScreen;

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

		public async Task LoadSceneSet(SceneContainer newSceneContainer)
		{
			_loadingScreen.Show();

			await UnloadOldScenes();

			_currSceneContainer = newSceneContainer;
			
			_mainSceneLoading = LoadSceneAsyncAdditive(_currSceneContainer.MainScene.Name);
			_uiSceneLoading = LoadSceneAsyncAdditive(_currSceneContainer.UIScene.Name);

			_mainSceneLoading.completed += FinishUILoading;
			_uiSceneLoading.completed += InvokeGlobalOnLevelLoaded;

			await DelayAllLoadings();

			PrepareTapToPlay();
		}

		private async Task UnloadOldScenes()
		{
			if (_currSceneContainer != null)
				await UnloadCurrentSceneSet();

			if (_mainMenuLoaded)
				await UnloadMainMenu();
		}

		private async Task DelayAllLoadings()
		{
			while (_mainSceneLoading.progress < 0.9f && _uiSceneLoading.progress < 0.9f || !_loadingScreen.IsShown)
				await Task.Yield();

			await Task.Delay(0.5f.AsMilliseconds());
		}

		private void PrepareTapToPlay()
		{
			_loadingScreen.SetTapOnTheScreenText();
			GlobalEventManager.OnScreenTap.AddListener(FinishLevelLoading);
		}

		private void FinishUILoading(AsyncOperation loading) =>
			_uiSceneLoading.allowSceneActivation = true;

		private void InvokeGlobalOnLevelLoaded(AsyncOperation loading)
		{
			GlobalEventManager.OnLevelLoaded?.Invoke();
			_mainSceneLoading.completed -= FinishUILoading;
			_uiSceneLoading.completed -= InvokeGlobalOnLevelLoaded;
		}

		private void FinishLevelLoading()
		{
			_mainSceneLoading.allowSceneActivation = true;
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