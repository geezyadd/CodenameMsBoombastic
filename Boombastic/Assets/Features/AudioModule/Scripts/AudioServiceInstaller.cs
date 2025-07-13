using Generations.Data;
using Plugins.Zenject.Source.Addons.AddressablesConfigurationsLoader;
using Zenject;

namespace Features.AudioModule.Scripts {
    public class AudioServiceInstaller : Installer<AudioServiceInstaller> {
        public override void InstallBindings()
        {
            BindConfigurations();
            BindAudioServices();
        }

        private void BindAudioServices()
        {
            Container.BindInterfacesAndSelfTo<FmodAudioService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<AudioVolumeService>()
                .AsSingle();
        }

        private void BindConfigurations() {
            Container.BindConfigurationFromAddressables<AudioEventReferenceConfiguration>(
                AddressableAssets.Configuration.AudioEventReferenceConfiguration).AsSingle();
        }

    }
}
