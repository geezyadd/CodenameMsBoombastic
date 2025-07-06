using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UIWorkflow.Core {
    public abstract class WindowBehaviour {
        private readonly IWindowFactory _windowFactory;
        private readonly IPresenterFactory _presenterFactory;
        private readonly List<PresenterBehaviour> _presenters = new();
        private GameObject _windowInstance;
        private WindowStatus _windowStatus;

        public WindowStatus WindowStatus => 
            _windowStatus;
        
        public IReadOnlyList<PresenterBehaviour> Presenters => 
            _presenters;

        internal WindowBehaviour(IWindowFactory windowFactory, IPresenterFactory presenterFactory) {
            _windowFactory = windowFactory;
            _presenterFactory = presenterFactory;
        }

        public void Show() {
            if (_windowStatus is WindowStatus.Showed) {
                Debug.LogWarning("Window already shown");
                return;
            }

            if (_windowStatus is WindowStatus.Hidden)
                _windowInstance.SetActive(true);
            else {
                _windowInstance = _windowFactory.Create(this);
                ViewBehaviour[] views = _windowInstance.GetComponentsInChildren<ViewBehaviour>();
                foreach (ViewBehaviour viewBehaviour in views)
                    RegisterView(viewBehaviour);
            }

            foreach (PresenterBehaviour presenter in _presenters)
                presenter.ShowView();
            
            _windowStatus = WindowStatus.Showed;
        }

        public void Hide() {
            if (_windowStatus is WindowStatus.Hidden or WindowStatus.None) {
                Debug.LogWarning("Window already hidden or closed");
                return;
            }
            
            foreach (PresenterBehaviour presenter in _presenters)
                presenter.HideView();
            
            _windowInstance.SetActive(false);
            _windowStatus = WindowStatus.Hidden;
        }

        public void Close() {
            if (_windowStatus is WindowStatus.None) {
                Debug.LogWarning("Window already closed");
                return;
            }

            foreach (PresenterBehaviour presenter in _presenters)
                presenter.CloseView();
            
            _presenters.Clear();
            Object.Destroy(_windowInstance);
            _windowStatus = WindowStatus.None;
        }

        public void RegisterView<TView>(TView view) where TView : ViewBehaviour {
            PresenterBehaviour presenter = _presenterFactory.CreateForView(view);
            _presenters.Add(presenter);
            
            Action viewCloseHandler = null;
            viewCloseHandler = () => {
                view.OnClose -= viewCloseHandler;
                _presenters.Remove(presenter);
            };

            view.OnClose += viewCloseHandler;
        }
    }
}