using System;
using System.Collections;
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
		private readonly ICoroutineRunner _coroutineRunner;
		private SceneContainer _currSceneContainer;
		private SceneContainer _newSceneContainer;
		private Action _onLoad;
		
		public SceneLoader(LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner) =>
			(_loadingScreen, _coroutineRunner) = (loadingScreen, coroutineRunner);

		public void Load(SceneSets set, Action onLoad)
		{
			_newSceneContainer = GetSceneContainer(set);
			_onLoad = onLoad;

			_loadingScreen.OnShown.AddListener(StartSceneLoadFlow);
			_loadingScreen.Show();
		}

		private void StartSceneLoadFlow()
		{
			if (_currSceneContainer != null)
				_coroutineRunner.StartCoroutine(UnloadSceneAsync());

			_coroutineRunner.StartCoroutine(LoadSceneAsync(_newSceneContainer, _onLoad));
		}

		private IEnumerator UnloadSceneAsync()
		{
			AsyncOperation unloadingMain = SceneManager.UnloadSceneAsync(_currSceneContainer.MainScene.Name);
			AsyncOperation unloadingUI = SceneManager.UnloadSceneAsync(_currSceneContainer.UIScene.Name);

			while (!unloadingMain.isDone && unloadingUI.isDone)
				yield return null;
		}

		private IEnumerator LoadSceneAsync(SceneContainer container, Action onLoad)
		{
			_currSceneContainer = container;
			
			AsyncOperation loadingMain = SceneManager.LoadSceneAsync(container.MainScene.Name, LoadSceneMode.Additive);
			AsyncOperation loadingUI = SceneManager.LoadSceneAsync(container.UIScene.Name, LoadSceneMode.Additive);

			while (!loadingMain.isDone
			       && !loadingUI.isDone)
				yield return null;

			FinishLoading();
		}

		private void FinishLoading()
		{
			_onLoad?.Invoke();
			_loadingScreen.Hide();
			_loadingScreen.OnShown.RemoveAllListeners();
		}

		private SceneContainer GetSceneContainer(SceneSets set) =>
			Services.ConfigService.ScenesConfig.GetSceneContainer(set);
	}
}