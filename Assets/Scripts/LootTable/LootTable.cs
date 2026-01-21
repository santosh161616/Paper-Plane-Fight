using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "LootTable", menuName = "Loot/LootTable")]
public class LootTable : ScriptableObject
{
    [Serializable]
    public class LootEntry
    {
        public string id;
        public GameObject prefab;            // Collectable prefab (should contain CollactableBase or Pickups)
        [Range(0f, 1f)] public float chance = 0.2f; // Probability (0..1) that this entry will drop
        public int minCount = 1;
        public int maxCount = 1;
        public float fallSpeed = 4f;         // Downwards speed (positive)
        public float scatterRadius = 0.5f;   // Random spawn offset radius
        public float popScale = 0.12f;       // Pop animation time
    }

    public List<LootEntry> entries = new List<LootEntry>();

    /// <summary>
    /// Spawn loot according to this table at world position.
    /// Returns list of spawned GameObjects.
    /// </summary>
    public List<GameObject> Spawn(Vector3 worldPosition)
    {
        var spawned = new List<GameObject>();

        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue;

            if (UnityEngine.Random.value > entry.chance) continue;

            int count = UnityEngine.Random.Range(entry.minCount, entry.maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                Vector2 offset = UnityEngine.Random.insideUnitCircle * entry.scatterRadius;
                Vector3 spawnPos = worldPosition + new Vector3(offset.x, offset.y, 0f);

                GameObject go = Instantiate(entry.prefab, spawnPos, Quaternion.identity);

                // Small pop animation for high quality feel
                try
                {
                    go.transform.localScale = Vector3.zero;
                    go.transform.DOScale(Vector3.one, entry.popScale).SetEase(DG.Tweening.Ease.OutBack);
                }
                catch (Exception)
                {
                    // DOTween might not be available at runtime � swallowing to keep robust.
                }

                // Set Rigidbody2D fall velocity if available
                var rb = go.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), -Mathf.Abs(entry.fallSpeed));
                }

                spawned.Add(go);
            }
        }

        return spawned;
    }
}
