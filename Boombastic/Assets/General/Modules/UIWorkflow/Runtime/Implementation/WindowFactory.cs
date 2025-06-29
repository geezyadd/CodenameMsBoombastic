using Features.AssetLoaderModule.Scripts;
using UIWorkflow.Core;
using UnityEngine;
using Zenject;

namespace UIWorkflow.Implementation {
    internal class WindowFactory : IWindowFactory {
        private readonly IAssetLoaderService _assetLoader;
        private readonly DiContainer _container;

        public WindowFactory(IAssetLoaderService assetLoader, DiContainer container) {
            _assetLoader = assetLoader;
            _container = container;
        }

        public GameObject Create(string windowName) {
            GameObject windowObject = _assetLoader.LoadAsset<GameObject>(windowName);
            return _container.InstantiatePrefab(windowObject);
        }

        public GameObject Create<TWindow>(TWindow windowType) where TWindow : WindowBehaviour {
            string windowName = windowType.GetType().Name;
            GameObject windowObject = _assetLoader.LoadAsset<GameObject>(windowName);
            return _container.InstantiatePrefab(windowObject);
        }
    }
}