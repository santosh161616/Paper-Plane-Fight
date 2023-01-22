using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [SerializeField] int addToHealth = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().HealthEarned(addToHealth); 
            Destroy(gameObject);
        }        
    }
}
