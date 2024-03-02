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
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject startGameCanvas, gameOverCanvas;
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
        spawner.SetActive(true);
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
        spawner.SetActive(false);
        startGameCanvas.SetActive(true);

    }
    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
