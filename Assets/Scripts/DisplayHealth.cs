using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    TextMeshProUGUI textHealth;

    Player player;
    void Start()
    {
        textHealth = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.text = player.GetHealth().ToString();
    }
}
