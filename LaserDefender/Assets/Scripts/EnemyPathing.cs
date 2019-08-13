using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    private WaveConfig waveConfig;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = this.waveConfig.Waypoints[this.waypointIndex].transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (this.waypointIndex < this.waveConfig.Waypoints.Count - 1)
        {
            Vector3 targetPosition = this.waveConfig.Waypoints[this.waypointIndex + 1].transform.position;

            transform.position = Vector3.MoveTowards(
                current: this.transform.position,
                target: targetPosition,
                maxDistanceDelta: this.waveConfig.MoveSpeed * Time.deltaTime
            );

            if (Vector3.Equals(this.transform.position, targetPosition))
                this.waypointIndex++;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
