using System;
using UnityEngine;

namespace Plane.popups
{
    public abstract class PopupBase : MonoBehaviour, IPopup
    {
        protected Action onYesAction;
        protected Action onNoAction;
        public virtual void Show(string message)
        {
            Show(message, "", null, null);
        }

        public virtual void Show(string message, string title)
        {
            Show(message, title, null, null);
        }

        public virtual void Show(string message, string title, Action onYes)
        {
            Show(message, title, onYes, null);
        }

        public virtual void Show(string message, string title, Action onYes, Action onNo)
        {
            onYesAction = onYes;
            onNoAction = onNo;

            // Implement UI display logic here
            UpdateUI(message, title);
            gameObject.SetActive(true);
            OnShow();

        }

        protected abstract void UpdateUI(string message, string title);
        protected abstract void OnShow();

        public virtual void OnYes()
        {
            onYesAction?.Invoke();
            Close();           
        }

        public virtual void OnNo()
        {
            onNoAction?.Invoke();
            Close();
        }

        public virtual void Close()
        {
            onNoAction = null;
            onYesAction = null;
            gameObject.SetActive(false);
        }
    }
}
