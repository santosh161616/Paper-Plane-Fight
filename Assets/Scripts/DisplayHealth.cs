using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Plane.Utils;

public class DisplayHealth : MonoBehaviour
{
    TextMeshProUGUI textHealth;

    void Start()
    {
        textHealth = GetComponent<TextMeshProUGUI>();
        GameEvents.Instance.OnUpdateHealthUI += UpdateHealthUI;
    }

    // Update is called once per frame
    void UpdateHealthUI(int healthValue)
    {
        textHealth.text = healthValue.ToString();
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnUpdateHealthUI -= UpdateHealthUI;
    }
}
