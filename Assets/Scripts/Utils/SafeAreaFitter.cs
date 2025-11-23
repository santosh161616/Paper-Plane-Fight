using UnityEngine;

namespace Plane.Utils
{

    [ExecuteAlways]
    public class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private bool simulateInEditor = false;

        private RectTransform panel;
        private Rect lastSafeArea = new Rect(0, 0, 0, 0);
        private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;
        private Vector2 lastResolution = Vector2.zero;

        private void Awake()
        {
            panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void Update()
        {
            if (Application.isEditor && !simulateInEditor)
                return;

            if (Screen.safeArea != lastSafeArea ||
                Screen.orientation != lastOrientation ||
                lastResolution.x != Screen.width ||
                lastResolution.y != Screen.height)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            if (panel == null)
                return;

            Rect safeArea = Screen.safeArea;

            // Save states
            lastSafeArea = safeArea;
            lastOrientation = Screen.orientation;
            lastResolution = new Vector2(Screen.width, Screen.height);

            // Convert safe area rectangle into anchor min/max
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;

            Debug.Log($"[SafeArea] Applied — Min: {anchorMin}, Max: {anchorMax}");
        }
    }
}
