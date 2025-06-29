using System;
using UIWorkflow.Core;
using Zenject;

namespace UIWorkflow.Implementation {
    internal class PresenterFactory : IPresenterFactory {
        private readonly DiContainer _container;

        public PresenterFactory(DiContainer container) =>
            _container = container;

        public TPresenter Create<TPresenter>() where TPresenter : PresenterBehaviour =>
            _container.Instantiate<TPresenter>();

        public PresenterBehaviour Create(Type presenterType) =>
            (PresenterBehaviour)_container.Instantiate(presenterType);

        public PresenterBehaviour CreateForView<TView>(TView view) where TView : ViewBehaviour {
            Type viewType = view.GetType();
            Type presenterType = Type.GetType($"{viewType.Namespace}.{viewType.Name.Replace("View", "Presenter")}, {viewType.Assembly}");
            PresenterBehaviour presenterBehaviour = Create(presenterType);
            presenterBehaviour.ViewBehaviour = view;
            return presenterBehaviour;
        }
    }
}