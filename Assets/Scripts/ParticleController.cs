using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem part;
    [SerializeField] List<ParticleCollisionEvent> collisionEvents;
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        Debug.Log("Particle Hit Player");
        int i = 0;
        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector2 pos = collisionEvents[i].intersection;
                Vector2 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }
    }
}
