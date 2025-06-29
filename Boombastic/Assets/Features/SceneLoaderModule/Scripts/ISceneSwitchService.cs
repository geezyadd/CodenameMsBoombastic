using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Features.SceneLoaderModule.Scripts {
    public interface ISceneSwitchService {
        public UniTask UpdateLoadedScenes(IReadOnlyList<string> newScenes, string newActiveScene, LoadSceneMode loadSceneMode);
    }
}