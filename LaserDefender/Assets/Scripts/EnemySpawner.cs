using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = true;

    [SerializeField] float difficulty = 1.0f;
    [SerializeField] float difficultyScale = 1.0f;

    IEnumerator Start()
    {
        while (this.looping)
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
    }

    private IEnumerator SpawnAllWaves()
    {
        foreach(var wave in waveConfigs)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(wave));
        }

        this.difficultyScale += difficultyScale;
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        waveConfig.SetDificulty(this.difficulty);

        for (int enemyCount = 0; enemyCount < waveConfig.NumberOfEnemies; enemyCount++)
        {
            GameObject newEnemy = Instantiate(
                original: waveConfig.EnemyPrefab,
                position: waveConfig.Waypoints[0].transform.position,
                rotation: Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.TimeBetweenSpawns);
        }
    }
}
