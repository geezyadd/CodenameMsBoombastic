using Features.AssetLoaderModule.Scripts;
using UnityEngine;
using Zenject;

namespace Plugins.Zenject.Source.Addons.AddressablesConfigurationsLoader {
    public static class AddressablesLoaderZenjectExtensions
    {
        public const string CONFIGURATIONS_GROUP_NAME = "Configurations";
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindConfigurationFromAddressables<T>(this DiContainer container, string addressableKey) where T : ScriptableObject {
            T configuration = container.Resolve<IAddressablesAssetLoaderService>().LoadAsset<T>(addressableKey, CONFIGURATIONS_GROUP_NAME);

            return container.Bind<T>().FromScriptableObject(configuration);
        }
        
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindConfigurationFromAddressablesToInterface<TConfiguration, TInterface>(this DiContainer container, string addressableKey) where TConfiguration : ScriptableObject, TInterface {
            TConfiguration configuration = container.Resolve<IAddressablesAssetLoaderService>().LoadAsset<TConfiguration>(addressableKey, CONFIGURATIONS_GROUP_NAME);

            return container.Bind<TInterface>().To<TConfiguration>().FromScriptableObject(configuration);
        }
    }
}