using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float endGameDelay = 2.5f;

    GameSession gameSession;

    private void Start()
    {
        this.gameSession = FindObjectOfType<GameSession>();
    }

    public void Menu()
    {
        this.gameSession.ResetGame();

        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        this.gameSession.ResetGame();

        this.StartGame();
    }

    public void End()
    {
        StartCoroutine(EndGame());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(this.endGameDelay);

        SceneManager.LoadScene(2);
    }
}
