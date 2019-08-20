using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    Text scoreText;
    GameSession gameSession;

    void Start()
    {
        this.scoreText = GetComponent<Text>();
        this.gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        this.scoreText.text = this.FormatScore();   
    }

    private string FormatScore()
    {
        return this.gameSession.Score.ToString("D" + 6);
    }
}
