using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Plane.Utils;

public class DisplayScore : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        GameEvents.Instance.OnScoreReceived += UpdateUI;
    }

    void UpdateUI(int score)
    {        
        scoreText.text = score.ToString();
    }
    private void OnDestroy()
    {
        GameEvents.Instance.OnScoreReceived -= UpdateUI;

    }
}
