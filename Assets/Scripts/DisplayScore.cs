using Plane.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        GameEvents.Instance.OnScoreReceived += UpdateUI;
    }

    private void Start()
    {
        ScoreManager.Load();
        UpdateUI(ScoreManager.CurrentScore);
    }

    private void UpdateUI(int score)
    {
        scoreText.text = score.ToString();
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnScoreReceived -= UpdateUI;
    }
}
