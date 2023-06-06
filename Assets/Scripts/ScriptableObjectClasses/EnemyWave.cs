using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Scriptable object that store information on an enemy wave.
/// Mainly stores the set of waypoints that make up the path.
/// </summary>
[CreateAssetMenu (fileName = "New Enemy Wave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField] private Transform pathPrefab;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float timeBetweenUfoSpawns = 1f;
    [SerializeField] private float spawnVariance = 0f;

    public List<GameObject> EnemyPrefabs => enemyPrefabs;

    public Transform PathPrefab
    {
        get => pathPrefab;
        set => pathPrefab = value;
    }

    public float MoveSpeed => moveSpeed;

    public Transform GetStartingWaypoint()
    {
        return pathPrefab.GetChild(0);
    }

    public List<Transform> GetWaypoints()
    {
        return pathPrefab.Cast<Transform>().ToList();
    }
    
    public GameObject GetEnemyPrefabAtIndex(int index)
    {
        return enemyPrefabs[index];
    }

    public float GetRandomSpawnTIme()
    {
        float spawnTime = Random.Range(timeBetweenUfoSpawns - spawnVariance,
            timeBetweenUfoSpawns + spawnVariance);

        return Mathf.Clamp(spawnTime, 0.4f, 10f);
    }
}
