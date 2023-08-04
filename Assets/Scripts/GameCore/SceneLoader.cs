using System.Threading.Tasks;
using Configs;
using GameCore.Events;
using GameCore.GameUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils.Constants;

namespace GameCore
{
	public sealed class SceneLoader
	{
		private readonly LoadingScreen _loadingScreen;
		public readonly UnityEvent OnLoaded = new();
		private SceneContainer _currSceneContainer;
		private AsyncOperation _mainSceneLoading;
		private AsyncOperation _uiSceneLoading;
		private bool _mainMenuLoaded;
		private bool _sceneSetLoaded;

		public SceneLoader(LoadingScreen loadingScreen) =>
			_loadingScreen = loadingScreen;

		public void LoadMainMenu()
		{
			_loadingScreen.Show();
			_loadingScreen.OnShown.AddListener(RunMainMenu);

			StartMainMenuLoading();
		}

		public void LoadSceneSet(SceneContainer newSceneContainer)
		{
			_loadingScreen.Show();
			_loadingScreen.OnShown.AddListener(RunSceneSet);
			
			StartSceneSetLoading(newSceneContainer);
		}

		private void StartMainMenuLoading()
		{
			UnloadCurrentSceneSet();
			
			_mainSceneLoading = LoadSceneAsyncAdditive(Scenes.MainMenuUI);
			_mainSceneLoading.completed += SetMainMenuLoaded;
			_mainSceneLoading.completed += HideLoadingScreen;
		}

		private async void RunMainMenu()
		{
			_loadingScreen.OnShown.RemoveListener(RunMainMenu);

			while (_mainSceneLoading.progress < 0.9f
			       || !_loadingScreen.IsShown
			       || _sceneSetLoaded)
			{
				await Task.Yield();
				Debug.Log("awaiting");
			}

			_mainSceneLoading.allowSceneActivation = true;
		}

		private void StartSceneSetLoading(SceneContainer newSceneContainer)
		{
			UnloadOldScenes();
			
			_mainSceneLoading = LoadSceneAsyncAdditive(newSceneContainer.MainScene.Name);
			_uiSceneLoading = LoadSceneAsyncAdditive(newSceneContainer.UIScene.Name);

			_mainSceneLoading.completed += FinishUILoading;
			_uiSceneLoading.completed += SetSceneSetLoaded;
			_uiSceneLoading.completed += InvokeGlobalOnLevelLoaded;
		}

		private async void RunSceneSet()
		{
			while (_mainSceneLoading.progress < 0.9f
			       && _uiSceneLoading.progress < 0.9f
			       || !_loadingScreen.IsShown
			       || _mainMenuLoaded
			       || _sceneSetLoaded)
				await Task.Yield();

			PrepareTapToPlay();
		}

		private void PrepareTapToPlay()
		{
			_loadingScreen.SetTapOnTheScreenText();
			GlobalEventManager.OnScreenTap.AddListener(FinishLevelLoading);
		}

		private void FinishUILoading(AsyncOperation loading)
		{
			loading.completed -= FinishUILoading;
			_uiSceneLoading.allowSceneActivation = true;
		}

		private void InvokeGlobalOnLevelLoaded(AsyncOperation loading)
		{
			loading.completed -= InvokeGlobalOnLevelLoaded;

			GlobalEventManager.OnLevelLoaded?.Invoke();
		}

		private void FinishLevelLoading()
		{
			GlobalEventManager.OnScreenTap.RemoveListener(FinishLevelLoading);
			
			_mainSceneLoading.allowSceneActivation = true;
			HideLoadingScreen();
		}
		
		private void SetMainMenuLoaded(AsyncOperation loading)
		{
			loading.completed -= SetMainMenuLoaded;
			_mainMenuLoaded = true;
			OnLoaded?.Invoke();
		}

		private void SetSceneSetLoaded(AsyncOperation loading)
		{
			loading.completed -= SetSceneSetLoaded;
			_sceneSetLoaded = true;
			OnLoaded?.Invoke();
		}

		private void HideLoadingScreen(AsyncOperation asyncOperation = null)
		{
			if (_loadingScreen.IsShown)
				_loadingScreen.Hide();
			else
				_loadingScreen.OnShown.AddListener(HideLoadingOnShown);
		}

		private void HideLoadingOnShown()
		{
			_loadingScreen.OnShown.RemoveListener(HideLoadingOnShown);
			_loadingScreen.Hide();
		}

		private AsyncOperation LoadSceneAsyncAdditive(string scene)
		{
			AsyncOperation loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			loading.allowSceneActivation = false;
			return loading;
		}

		private void UnloadOldScenes()
		{
			UnloadMainMenu();
			UnloadCurrentSceneSet();
		}

		private async void UnloadMainMenu()
		{
			if (!_mainMenuLoaded)
				return;

			await UnloadSceneAsync(Scenes.MainMenuUI);
			_mainMenuLoaded = false;
		}

		private async void UnloadCurrentSceneSet()
		{
			if (!_sceneSetLoaded)
				return;

			await UnloadSceneAsync(_currSceneContainer.MainScene.Name);
			await UnloadSceneAsync(_currSceneContainer.UIScene.Name);
			_currSceneContainer = null;
			_sceneSetLoaded = false;

			GlobalEventManager.OnLevelUnloaded?.Invoke();
		}

		private async Task UnloadSceneAsync(string sceneName)
		{
			AsyncOperation unloading = SceneManager.UnloadSceneAsync(sceneName);
			while (!unloading.isDone)
				await Task.Yield();
		}
	}
}