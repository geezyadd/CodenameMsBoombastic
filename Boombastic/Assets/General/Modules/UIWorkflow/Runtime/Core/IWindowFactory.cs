using UnityEngine;

namespace UIWorkflow.Core {
    public interface IWindowFactory {
        public GameObject Create<TWindow>(TWindow windowType) where TWindow : WindowBehaviour;
    }
}