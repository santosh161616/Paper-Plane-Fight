using UnityEngine;

namespace collectables
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class CollactableBase : MonoBehaviour, ICollectable
    {
        [Header("Collectable Settings")]
        [SerializeField] protected bool destroyOnCollect = true;
        public abstract void ApplyEffect(GameObject collector);
        public void PickUp(GameObject collector)
        {
            ApplyEffect(collector);

            if (destroyOnCollect)
                Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PickUp(collision.gameObject);
            }
        }
    }
}
