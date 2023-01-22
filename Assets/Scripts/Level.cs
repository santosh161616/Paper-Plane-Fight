using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel1()
    {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene("Level 1");
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAfterPlayerDied());        
    }

    IEnumerator WaitAfterPlayerDied()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game Over");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
   
}
