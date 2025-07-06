using UnityEngine;

namespace UIWorkflow.Core {
    internal interface IWindowFactory {
        public GameObject Create<TWindow>(TWindow windowType) where TWindow : WindowBehaviour;
    }
}