using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PickupCoin : MonoBehaviour
{
    [SerializeField] int coinToReward = 1;
    [SerializeField] private float magnetStrength = 10f;
    [SerializeField] private float magnetRange = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameSession>().RewardCoin(coinToReward);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRange);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = transform.position - col.transform.position;
                    float distance = direction.magnitude;
                    float forceMagnitude = magnetStrength * (1 - distance / magnetRange);
                    Vector2 force = direction.normalized * forceMagnitude;
                    rb.AddForce(force);
                }

            }
        }
    }
}
