using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Plane.Utils;

public class GameSession : SingletonMonoBehaviour<GameSession>
{
    int score = 0;
    [SerializeField] int earnedCoin;

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] float coinPickUPSFXVolume = 0.4f;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameObject startGameCanvas, gameOverCanvas;
    [SerializeField] private GameObject playerSpawnPoint;
    [SerializeField] private List<Player> players;

    [Header("Health")]
    [SerializeField] public Image[] hearts;
    [SerializeField] Sprite fullHearts;
    [SerializeField] Sprite emptyHearts;

    public void StartGame()
    {
        SelectCurrentPlayer(0);
        StartCoroutine(spawner.StartWaves());
        startGameCanvas.SetActive(false);

        //Events
        GameEvents.Instance.OnRewardReceived += RewardCoin;
        GameEvents.Instance.OnUpdateHealthUI += UpdateHealthUI;
        GameEvents.Instance.OnGameOver += GameOver;
        GameEvents.Instance.OnAddtoScore += AddToScore;
        GameEvents.Instance.OnScoreReceived += GetScore;

    }

    private void SelectCurrentPlayer(int index)
    {
        if (!players[index].gameObject.activeInHierarchy)
        {
            var player = Instantiate(players[index], playerSpawnPoint.transform.position, Quaternion.identity);
            //player.GetComponent<Player>().HealthEarned(health: 4);
            GameEvents.Instance.HealthReceived(healthAmount: 4);
            //players[index].HealthEarned(health: 4);
        }
    }
    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }
        }
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

    private void OnDestroy()
    {
        GameEvents.Instance.OnRewardReceived -= RewardCoin;
        GameEvents.Instance.OnUpdateHealthUI -= UpdateHealthUI;
        GameEvents.Instance.OnGameOver -= GameOver;
        GameEvents.Instance.OnAddtoScore -= AddToScore;
        GameEvents.Instance.OnScoreReceived -= GetScore;
    }
}
