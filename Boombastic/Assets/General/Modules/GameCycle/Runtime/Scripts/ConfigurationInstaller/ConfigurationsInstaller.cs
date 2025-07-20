using Features.AssetLoaderModule.Scripts;
using Features.AudioModule.Scripts;
using Generations.Data;
using UnityEngine;
using Zenject;

namespace GameCycle.ConfigurationInstaller
{
    public class ConfigurationsInstaller : Installer<ConfigurationsInstaller>
    {
        public const string CONFIGURATIONS_GROUP_NAME = "Configurations";
        public override void InstallBindings()
        {
            BindConfigurationsFromAddressables<AudioEventReferenceConfiguration>(AddressableAssets.Configuration.AudioEventReferenceConfiguration);
        }

        private void BindConfigurationsFromAddressables<T>(string addressableKey) where T : ScriptableObject {
            T configuration =Container.Resolve<IAddressablesAssetLoaderService>().LoadAsset<T>(addressableKey, CONFIGURATIONS_GROUP_NAME);
            Container.Bind<T>().FromScriptableObject(configuration);
        }
    }
}
