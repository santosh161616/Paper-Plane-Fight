using System;
using UnityEngine;

namespace collectables
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class CollactableBase : MonoBehaviour, ICollectable
    {
        public abstract void PickUp();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PickUp();
                Destroy(gameObject);
            }
        }
    }
}
