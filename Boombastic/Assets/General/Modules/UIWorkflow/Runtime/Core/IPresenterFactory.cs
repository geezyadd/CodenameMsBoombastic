using System;

namespace UIWorkflow.Core {
    public interface IPresenterFactory {
        public PresenterBehaviour Create(Type presenterType);
        public PresenterBehaviour CreateForView<TView>(TView view) where TView : ViewBehaviour;
    }
}