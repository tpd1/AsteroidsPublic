using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the level timer and events that happen during the level such as UFO spawns.
/// </summary>
public class TimelineController : MonoBehaviour
{
    [SerializeField] private float maxTime = 100f;
    [SerializeField] private float ufoSpawnPercOne = 30f;
    [SerializeField] private float ufoSpawnPercTwo = 70f;
    [SerializeField] private float timeBetweenUfoWaves;
    [SerializeField] private List<EnemyWave> enemyWaves;

    private float currentTime;
    private float currentPercent;
    private bool waveTwoTriggered;
    private bool waveOneTriggered;
    private bool isPlaying = true;

    public EnemyWave CurrentWave { get; private set; }

    /// <summary>
    /// Check for ufo spawn times or end of level.
    /// </summary>
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime <= maxTime && isPlaying)
        {
            currentPercent = (currentTime / maxTime * 100.0f);
            if (currentPercent >= ufoSpawnPercOne && !waveOneTriggered && currentPercent < ufoSpawnPercTwo)
            {
                waveOneTriggered = true;
                StartCoroutine(TriggerUFOSpawn());
            }
            else if (currentPercent >= ufoSpawnPercTwo && !waveTwoTriggered)
            {
                waveTwoTriggered = true;
                StartCoroutine(TriggerUFOSpawn());
            }
        }
        else
        {
            if (isPlaying)
            {
                StopAllCoroutines();
                isPlaying = false;
                LevelManager.Instance.EndLevel();
            }
        }
    }

    public float GetCurrentPercent()
    {
        return currentPercent;
    }

    /// <summary>
    /// Begin a number of set waves of UFO enemies from set directions, based on scriptable objects.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator TriggerUFOSpawn()
    {
        foreach (var w in enemyWaves)
        {
            CurrentWave = w;
            for (int i = 0; i < CurrentWave.EnemyPrefabs.Count; i++)
            {
                Instantiate(CurrentWave.GetEnemyPrefabAtIndex(i),
                    CurrentWave.GetStartingWaypoint().position,
                    Quaternion.identity);
                yield return new WaitForSeconds(CurrentWave.GetRandomSpawnTIme());
            }
        }

        yield return new WaitForSeconds(timeBetweenUfoWaves);
    }
}