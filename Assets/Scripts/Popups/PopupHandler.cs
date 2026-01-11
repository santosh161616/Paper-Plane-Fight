using System.Collections.Generic;
using UnityEngine;

namespace Plane.popups
{
    public class PopupHandler : MonoBehaviour
    {
        public static PopupHandler Instance { get; private set; }

        [SerializeField] private GeneralPopup _popupPrefab;
        [SerializeField] private Transform _safeArea;
        private readonly Stack<PopupBase> popupStack = new Stack<PopupBase>();

        #region Singleton
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }
        #endregion

        public T ShowPopup<T>() where T : PopupBase
        {
            T popup = Instantiate(_popupPrefab, _safeArea) as T;
            popupStack.Push(popup);
            return popup;
        }

        public void CloseTopPopup()
        {
            if (popupStack.Count == 0)
                return;

            var popup = popupStack.Pop();
            popup.Close();
            Destroy(popup.gameObject);
        }

        private void Start()
        {
            //ShowPopup<GeneralPopup>()
            //    .Show("Welcome to the game!", "Welcome", 
            //    onYes: () => Debug.Log("Player clicked Yes"), 
            //    onNo: () => Debug.Log("Player clicked No"));
        }

        #region SimpleHandling
        //[SerializeField] private PopupBase popupPrefab;
        //private PopupBase currentPopup;
        //private void Awake()
        //{
        //    if (Instance == null)
        //    {
        //        Instance = this;
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        //public void ShowPopup(string message, string title = "", System.Action onYes = null, System.Action onNo = null)
        //{
        //    if (currentPopup != null)
        //    {
        //        Debug.LogWarning("A popup is already being displayed.");
        //        return;
        //    }
        //    currentPopup = Instantiate(popupPrefab, transform);
        //    currentPopup.Show(message, title, onYes, onNo);
        //}
        //public void CloseCurrentPopup()
        //{
        //    if (currentPopup != null)
        //    {
        //        currentPopup.Close();
        //        Destroy(currentPopup.gameObject);
        //        currentPopup = null;
        //    }
        //}
        #endregion
    }
}
