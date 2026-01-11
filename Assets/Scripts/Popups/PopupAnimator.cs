using DG.Tweening;
using UnityEngine;

namespace Plane.popups
{
    public class PopupAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform popupRoot;
        [SerializeField] private CanvasGroup popupCanvas;
        [SerializeField] private CanvasGroup backgroundCanvas;
        [Header("Settings")]
        [SerializeField] private float openDuration = 0.5f;
        [SerializeField] private float closeDuration = 0.5f;

        private Sequence sequence;

        private void ResetState()
        {
            popupRoot.localScale = Vector3.one * 0.85f;
            popupRoot.anchoredPosition = new Vector2(0, -40f);
            popupCanvas.alpha = 0f;
            if (backgroundCanvas) backgroundCanvas.alpha = 0f;
        }

        public void PlayOpen()
        {
            gameObject.SetActive(true);
            ResetState();

            sequence?.Kill();
            sequence = DOTween.Sequence();

            // Background fade (fast)
            if (backgroundCanvas)
                sequence.Join(backgroundCanvas.DOFade(0.75f, openDuration * 0.6f));

            // Start smaller & lower
            popupRoot.localScale = Vector3.one * 0.75f;
            popupRoot.anchoredPosition = new Vector2(0, -80f);

            // Pop in
            sequence.Append(
                popupRoot.DOScale(1.15f, openDuration * 0.45f)
                         .SetEase(Ease.OutBack)
            );

            sequence.Join(
                popupCanvas.DOFade(1f, openDuration * 0.4f)
            );

            sequence.Join(
                popupRoot.DOAnchorPosY(20f, openDuration * 0.45f)
                         .SetEase(Ease.OutCubic)
            );

            // Snap back
            sequence.Append(
                popupRoot.DOScale(0.97f, openDuration * 0.2f)
                         .SetEase(Ease.InOutSine)
            );

            // Settle
            sequence.Append(
                popupRoot.DOScale(1f, openDuration * 0.15f)
            );

            // Micro punch (feels alive)
            sequence.Append(
                popupRoot.DOPunchScale(Vector3.one * 0.03f, 0.15f, 6, 0.8f)
            );
        }


        public void PlayClose(System.Action onComplete)
        {
            sequence?.Kill();
            sequence = DOTween.Sequence();

            // Quick shrink + drop
            sequence.Append(
                popupRoot.DOScale(0.85f, closeDuration * 0.6f)
                         .SetEase(Ease.InBack)
            );

            sequence.Join(
                popupRoot.DOAnchorPosY(-100f, closeDuration * 0.6f)
                         .SetEase(Ease.InCubic)
            );

            sequence.Join(
                popupCanvas.DOFade(0f, closeDuration * 0.6f)
            );

            if (backgroundCanvas)
                sequence.Join(backgroundCanvas.DOFade(0f, closeDuration));

            sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

    }
}
