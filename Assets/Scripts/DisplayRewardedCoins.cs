using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Plane.Utils;
public class DisplayRewardedCoins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        GameEvents.Instance.OnGetCoin += UpdateCoinUI;
    }

    void UpdateCoinUI(int coin)
    {
        textUI.text = coin.ToString();
    }


    private void OnDestroy()
    {
        GameEvents.Instance.OnGetCoin -= UpdateCoinUI;
    }
}
