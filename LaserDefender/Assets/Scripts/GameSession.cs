using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    int score = 0;

    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }

    public int Score
    {
        get
        {
            return this.score;
        }
    }

    public void AddToScore(int scoreValue) => this.score += scoreValue;

    public void ResetGame() => Destroy(this.gameObject);
}
