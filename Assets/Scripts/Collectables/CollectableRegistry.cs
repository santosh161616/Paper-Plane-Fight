using System.Collections.Generic;
using UnityEngine;

namespace collectables
{
    [CreateAssetMenu(fileName = "CollectableRegistry", menuName = "ScriptableObject/CollectableRegistry")]
    public class CollectableRegistry : ScriptableObject
    {
        public List<CollectableEntry> entries;

        [System.Serializable]
        public class CollectableEntry
        {
            public CollactableType collactableType;
            public string id;
            public Sprite icon;
            public string name;
            public GameObject prefab;
            public float spawnProbability; // Value between 0 and 1
        }

        public GameObject GetCollectablePrefab(CollactableType type)
        {
            var entry = entries.Find(e => e.collactableType == type);
            return entry != null ? entry.prefab : null;
        }

    }
    public enum CollactableType
    {
        Consumable,
        Equipment
    }

    public enum ConsumableType
    {
        Health,
        Mana,
        Stamina,
        Ammo,
        Coin,
        Gem,
        PowerUp,
        HealthPack,
        Shield,
        SpeedBoost
    }

    public enum EquipmentType
    {
        Weapon,
        Armor,
        Accessory,
        Tool
    }
}
