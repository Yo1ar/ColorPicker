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
		private SceneSet _currSceneSet;
		private readonly LoadingScreen _loadingScreen;

		public SceneLoader(LoadingScreen loadingScreen) =>
			_loadingScreen = loadingScreen;

		public async Task LoadSceneSet(SceneSet newSceneSet, Action onLoadMain = null)
		{
			_loadingScreen.Show();

			if (_currSceneSet != null)
				await UnloadCurrentSceneSet();

			_currSceneSet = newSceneSet;

			if (newSceneSet.set != SceneSets.MainMenu)
			{
				await LoadAdditive(_currSceneSet.mainScene);
				onLoadMain?.Invoke();
			}
			await LoadAdditive(_currSceneSet.uiScene);

			_loadingScreen.Hide();
		}

		private async Task LoadAdditive(SceneField scene)
		{
			AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
			while (!sceneLoad.isDone)
				await Task.Yield();
		}

		private async Task UnloadCurrentSceneSet()
		{
			if (_currSceneSet.set != SceneSets.MainMenu)
			{
				AsyncOperation sceneUnload1 = SceneManager.UnloadSceneAsync(_currSceneSet.mainScene.Name);
				while (sceneUnload1.isDone)
					await Task.Yield();
			}

			AsyncOperation sceneUnload2 = SceneManager.UnloadSceneAsync(_currSceneSet.uiScene.Name);
			while (sceneUnload2.isDone)
				await Task.Yield();
		}
	}
}