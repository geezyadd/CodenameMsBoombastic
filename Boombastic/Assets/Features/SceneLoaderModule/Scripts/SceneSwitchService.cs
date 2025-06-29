using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Features.SceneLoaderModule.Scripts {
    public class SceneSwitchService : ISceneSwitchService {
        public Dictionary<string, AsyncOperationHandle<SceneInstance>> LoadedScenes { get; set; } = new();
        public event Action<string> OnSceneLoaded;

        public async UniTask UpdateLoadedScenes(IReadOnlyList<string> newScenes, string newActiveScene, LoadSceneMode loadSceneMode)
        {
            await UnloadRedundantScenes(newScenes);
            await LoadScenes(newScenes, loadSceneMode);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newActiveScene));
        }

        private async UniTask LoadScenes(IReadOnlyList<string> newScenes, LoadSceneMode loadSceneMode)
        {
            foreach (string newScene in newScenes)
            {
                if (IsSceneLoaded(newScene))
                    continue;

                AsyncOperationHandle<SceneInstance> asyncOperationHandler = Addressables.LoadSceneAsync(newScene, loadSceneMode);
                LoadedScenes.Add(newScene, asyncOperationHandler);
                await asyncOperationHandler.Task;
                OnSceneLoaded?.Invoke(newScene);
            }
        }

        private async UniTask UnloadRedundantScenes(IReadOnlyList<string> newScenes)
        {
            List<string> scenesToUnload = (from loadedScene in LoadedScenes where !newScenes.Contains(loadedScene.Key) select loadedScene.Key).ToList();

            foreach (string sceneToUnload in scenesToUnload)
            {
                AsyncOperationHandle<SceneInstance> asyncOperationHandler = Addressables.UnloadSceneAsync(LoadedScenes[sceneToUnload]);
                LoadedScenes.Remove(sceneToUnload);
                await asyncOperationHandler.Task;
            }
        }

        private bool IsSceneLoaded(string sceneName) =>
            LoadedScenes.ContainsKey(sceneName);
    }
}
