using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    [SerializeField] int earnedCoin;

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] float coinPickUPSFXVolume = 0.4f;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameObject startGameCanvas, gameOverCanvas;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        SetupSingleton();
    }

    public void SetupSingleton()
    {
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartGame()
    {
        if (!player.gameObject.activeInHierarchy)
        {
            player.gameObject.SetActive(true);
            player.HealthEarned(health: 4);
        }
        StartCoroutine(spawner.StartWaves());
        startGameCanvas.SetActive(false);
    }
    public int GetScore()
    {
        return score;        
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void RewardCoin(int coin)
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, coinPickUPSFXVolume);
        earnedCoin += coin;
    }

    public int GetCoin()
    {
        return earnedCoin;
    }

    public async void GameOver()
    {
        gameOverCanvas.SetActive(true);
        await Task.Delay(3000);
        gameOverCanvas.SetActive(false);
        spawner.StopAllCoroutines();
        startGameCanvas.SetActive(true);

    }
    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
