using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = true;

    [SerializeField] float difficulty = 1.0f;
    [SerializeField] float difficultyScale = 0.2f;

    IEnumerator Start()
    {
        while (this.looping)
        {
            yield return StartCoroutine(SpawnAllWaves());

            this.difficulty += difficultyScale;
        }
    }

    private IEnumerator SpawnAllWaves()
    {
        foreach(WaveConfig wave in waveConfigs)
        {
            WaveConfig clone = UnityEngine.Object.Instantiate(wave) as WaveConfig;

            yield return StartCoroutine(SpawnAllEnemiesInWave(clone));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        waveConfig.SetDificulty(this.difficulty);

        bool startFromEnd = UnityEngine.Random.Range(0, 2) == 0;
        bool reverseY = UnityEngine.Random.Range(0, 2) == 0;

        for (int enemyCount = 0; enemyCount < waveConfig.NumberOfEnemies; enemyCount++)
        {
            int index = startFromEnd ? waveConfig.Waypoints.Count - 1 : 0;

            GameObject newEnemy = Instantiate(
                original: waveConfig.EnemyPrefab,
                position: new Vector3(
                    x: waveConfig.Waypoints[index].transform.position.x,
                    y: reverseY ?
                        -waveConfig.Waypoints[index].transform.position.y :
                        waveConfig.Waypoints[index].transform.position.y,
                    z: waveConfig.Waypoints[index].transform.position.z),
                rotation: Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig, startFromEnd, reverseY);

            yield return new WaitForSeconds(waveConfig.TimeBetweenSpawns);
        }
    }
}
