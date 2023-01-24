using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayRewardedCoins : MonoBehaviour
{
    GameSession gameSession;
    [SerializeField] TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = gameSession.GetCoin().ToString();
    }
}
