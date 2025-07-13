using SceneLoaderModule;
using UnityEngine;
using Zenject;

namespace GameCycle {
    [CreateAssetMenu(fileName = nameof(ProjectContextInstaller) + "_Default", menuName = "Configurations/GameCycle/" + nameof(ProjectContextInstaller))]
    public class ProjectContextInstaller : ScriptableObjectInstaller {
        public override void InstallBindings() {
            SceneLoaderInstaller.Install(Container);
        }
    }
}
