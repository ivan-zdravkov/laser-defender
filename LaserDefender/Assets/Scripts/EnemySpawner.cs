using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;

    int startingWave = 0;
    WaveConfig currentWave;

    // Start is called before the first frame update
    void Start()
    {
        this.currentWave = this.waveConfigs[this.startingWave];

        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
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
