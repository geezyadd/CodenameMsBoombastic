namespace UIWorkflow.Core {
    public abstract class PresenterBehaviour {
        internal ViewBehaviour ViewBehaviour;

        public abstract void ShowView();
        public abstract void HideView();
        public abstract void CloseView();
        protected abstract void OnViewShowed();
        protected abstract void OnViewHidden();
        protected abstract void OnViewClosed();
    }

    public abstract class PresenterBehaviour<TView> : PresenterBehaviour where TView : ViewBehaviour {
        private TView _view;
        protected TView View =>
            _view ??= ViewBehaviour as TView;

        public override void ShowView() {
            View.Show();
            OnViewShowed();
        }

        public override void HideView() {
            View.Hide();
            OnViewHidden();
        }
        
        public override void CloseView() {
            View.Close();
            OnViewClosed();
        }

        protected override void OnViewShowed() { }
        protected override void OnViewHidden() { }
        protected override void OnViewClosed() { }
    }
}