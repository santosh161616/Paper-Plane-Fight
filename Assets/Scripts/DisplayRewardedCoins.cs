using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayRewardedCoins : MonoBehaviour
{
    Player player;
    TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = player.GetCoin().ToString();
    }
}
