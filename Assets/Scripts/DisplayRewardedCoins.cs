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
        GameEvents.Instance.OnGetCoin += UpdateCoinUI;
        textUI.text = CoinManager.Coins.ToString();
    }

    void UpdateCoinUI(int coin)
    {
        CoinManager.AddCoins(coin);
        textUI.text = CoinManager.Coins.ToString();
    }


    private void OnDestroy()
    {
        GameEvents.Instance.OnGetCoin -= UpdateCoinUI;
    }
}
