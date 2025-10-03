using UnityEngine;

namespace collectables
{
    public class CollectableManager : MonoBehaviour
    {
        [SerializeField] private CollectableFactory _factory;
        [SerializeField] private CollectableRegistry _registry;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnIntervals = 10f;

        private float _timer;

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnIntervals)
            {
                SpawnCollectable();
                _timer = 0f;
            }
        }

        private void SpawnCollectable()
        {
            if (_spawnPoints.Length == 0 || _registry.entries.Count == 0) return;
            // Select a random spawn point
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            // Select a collectable based on spawn probabilities
            float rand = Random.Range(0f, 1f);
            float cumulative = 0f;
            foreach (var entry in _registry.entries)
            {
                cumulative += entry.spawnProbability;
                if (rand <= cumulative)
                {
                    _factory.CreateCollectable(entry.collactableType, spawnPoint.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
