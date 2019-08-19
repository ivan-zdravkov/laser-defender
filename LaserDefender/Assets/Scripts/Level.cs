using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float endGameDelay = 2.5f;

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Game()
    {
        SceneManager.LoadScene(1);
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
