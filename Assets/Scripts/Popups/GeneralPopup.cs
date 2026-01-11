using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plane.popups
{
    public class GeneralPopup : PopupBase
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        protected override void OnShow()
        {
            _yesButton.onClick.AddListener(OnYes);
            _noButton.onClick.AddListener(OnNo);
        }

        protected override void UpdateUI(string message, string title)
        {
            _titleText.text = string.IsNullOrEmpty(title) ? "" : title;
            _messageText.text = message;

            _noButton.gameObject.SetActive(onNoAction != null);
        }

        public override void Close()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
            base.Close();
        }
    }
}
