using System;

namespace UIWorkflow.Core {
    internal interface IPresenterFactory {
        public PresenterBehaviour Create(Type presenterType);
        public PresenterBehaviour CreateForView<TView>(TView view) where TView : ViewBehaviour;
    }
}