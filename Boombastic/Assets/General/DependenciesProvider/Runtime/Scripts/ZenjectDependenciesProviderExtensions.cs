using System;
using Zenject;

namespace DependenciesProvider {
    public static class ZenjectDependenciesProviderExtensions {
        public static ConcreteIdBinderGeneric<TContract> BindAndRegisterInProvider<TContract>(this DiContainer container) =>
            container.Bind<TContract>().RegisterInProvider();

        private static ConcreteIdBinderGeneric<TContract> RegisterInProvider<TContract>(this ConcreteIdBinderGeneric<TContract> binder) {
            binder.BindInfo.NonLazy = true;
            binder.OnInstantiated<TContract>((context, instance) => {
                ZenjectDependenciesProvider.Register(instance);
                
                context.Container.Bind<IDisposable>()
                       .To<ZenjectDependencyUnregister<TContract>>()
                       .AsSingle()
                       .NonLazy();
            });

            return binder;
        }

        private class ZenjectDependencyUnregister<TContract> : IDisposable {
            public void Dispose() =>
                ZenjectDependenciesProvider.Unregister<TContract>();
        }
    }
}