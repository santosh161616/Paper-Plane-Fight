using UnityEngine;

public class PickupCoin : MonoBehaviour
{
    [SerializeField] int coinToReward = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameSession>().RewardCoin(coinToReward);
            Destroy(gameObject);
        }
    }
    
}
