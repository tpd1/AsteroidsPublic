using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the enemy movement from waypoint to waypoint.
/// </summary>
public class EnemyPathfinding : MonoBehaviour
{
    private EnemyWave config;
    private TimelineController timelineController;
    private List<Transform> waypoints;
    private int waypointIndex;

    private void Start()
    {
        timelineController = FindObjectOfType<TimelineController>();
        config = timelineController.CurrentWave;
        waypoints = config.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }

    private void Update()
    {
        ContinuePath();
    }

    private void ContinuePath()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPos = waypoints[waypointIndex].position;
            float posDelta = config.MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, posDelta);

            // If we have reached the waypoint, move to next one.
            if (transform.position == targetPos)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}