using UnityEngine;

namespace collectables
{
    public class CollectableFactory : MonoBehaviour
    {
       [SerializeField] private CollectableRegistry _registry;
        public GameObject CreateCollectable(CollactableType type, Vector3 position, Quaternion rotation)
        {
            var prefab = _registry.GetCollectablePrefab(type);
            if (prefab != null)
            {
                return Instantiate(prefab, position, rotation);
            }
            Debug.LogWarning($"Collectable prefab for type {type} not found.");
            return null;
        }
    }
}
