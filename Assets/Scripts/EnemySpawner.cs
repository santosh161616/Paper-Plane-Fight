using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool isLooping = false;
    int startingWave = 0;
    public int totalEnemies;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (isLooping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            // TO check the total number of Enemies in Level
            if (waveIndex == 0)
            {
                for (int i = 0; i < waveConfigs.Count; i++)
                {
                    totalEnemies += waveConfigs[i].GetNumberOfEnemies();
                }
                Debug.Log("Total Number of Enemies: " + totalEnemies);
            }
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
          
        }
    }
    
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
            waveConfig.GetEnemyPrefab(),
            waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
            
        }
    }

    public bool AllEnemiesDead()
    {
        Enemy[] childs = FindObjectsOfType<Enemy>();
        if (childs.Length > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
