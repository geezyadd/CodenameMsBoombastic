using System;
using UnityEngine;

namespace UIWorkflow.Core {
    public abstract class ViewBehaviour : MonoBehaviour {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnClose;
        public event Action OnShown;
        public event Action OnHidden;
        public event Action OnClosed;
        
        public virtual void Show() {
            OnShow?.Invoke();
            ShowInstance();
            OnShown?.Invoke();
        }

        public void Hide() {
            OnHide?.Invoke();
            HideInstance();
            OnHidden?.Invoke();
        }

        public void Close() {
            OnClose?.Invoke();
            CloseInstance();
            OnClosed?.Invoke();
        }
        
        protected virtual void ShowInstance() =>
            gameObject.SetActive(true);
        
        protected virtual void HideInstance() =>
            gameObject.SetActive(false);

        protected virtual void CloseInstance() =>
            Destroy(gameObject);
    }
}