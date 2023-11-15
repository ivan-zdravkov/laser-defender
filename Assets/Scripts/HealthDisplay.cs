using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text healthText;
    Player player;

    void Start()
    {
        this.healthText = GetComponent<Text>();
        this.player = FindObjectOfType<Player>();
    }

    void Update()
    {
        this.healthText.text = this.player.Health.ToString();
    }
}
