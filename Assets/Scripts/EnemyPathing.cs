using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    private WaveConfig waveConfig;
    private int waypointIndex = 0;

    List<Transform> waypoints;
    bool startFromEnd;
    bool reverseY;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = this.waypoints[this.waypointIndex].transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig, bool startFromEnd, bool reverseY)
    {
        this.waveConfig = waveConfig;
        this.startFromEnd = startFromEnd;
        this.reverseY = reverseY;

        this.waypoints = this.waveConfig.Waypoints;

        if (this.startFromEnd)
            this.waypoints.Reverse();

        if (this.reverseY)
            foreach (Transform waypoint in waypoints)
                waypoint.position.Set(waypoint.position.x, -waypoint.position.y, waypoint.position.z);
    }

    private void Move()
    {
        if (this.waypointIndex < this.waypoints.Count - 1)
        {
            Vector3 targetPosition = this.waypoints[this.waypointIndex + 1].transform.position;

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
