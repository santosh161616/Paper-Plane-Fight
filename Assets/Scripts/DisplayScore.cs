using Plane.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        GameEvents.Instance.OnScoreReceived += UpdateScore;
    }

    private void Start()
    {
        ScoreManager.Load();
        UpdateUI(ScoreManager.CurrentScore);
    }

    private void UpdateScore(int score)
    {
        ScoreManager.Add(score); // Trigger save and event
        UpdateUI(ScoreManager.CurrentScore);
    }

    private void UpdateUI(int score)
    {
        scoreText.text = score.ToString();
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnScoreReceived -= UpdateScore;
    }
}
