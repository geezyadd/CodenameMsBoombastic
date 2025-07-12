using DependenciesProvider;
using Zenject;

namespace SceneLoaderModule {
    public class SceneLoaderInstaller : Installer<SceneLoaderInstaller> {
        public override void InstallBindings() {
            Container.BindAndRegisterInProvider<ISceneSwitchService>()
                     .To<AddressablesSceneSwitchService>()
                     .AsSingle();
        }
    }
}