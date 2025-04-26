using Plane.Utils;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] bool _isHealthPickup;
    [SerializeField] int _coinValue = 1;
    [SerializeField] int _healthValue = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_isHealthPickup)
            {
                GameEvents.Instance.HealthReceived(_healthValue);
            }
            else
            {
                GameEvents.Instance.RewardReceived(_coinValue);
            }
            Destroy(gameObject);
        }
    }

}
