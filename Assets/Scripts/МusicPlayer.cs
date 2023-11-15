using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class МusicPlayer : MonoBehaviour
{
    void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
}
