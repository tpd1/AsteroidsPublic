using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Controls the position and rate that asteroids are spawned at.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    public List<Asteroid> LargeAsteroids { get; } = new();
    public List<Asteroid> SmallAsteroids { get; } = new();

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private int maxAsteroids = 5;
    [SerializeField] private float respawnRate = 5f;

    private Vector2 screenSize;
    private float minX, minY, maxX, maxY;
    private const float xOffset = 5f;
    private const float yOffset = 5f;

    private void Start()
    {
        // Define the spawn positions for the asteroids based on the screen size.
        screenSize = ScreenBounds.Instance.GetBoundsSize();
        minX = transform.position.x - screenSize.x / 2.0f;
        maxX = transform.position.x + screenSize.x / 2.0f;
        minY = transform.position.y - screenSize.y / 2.0f;
        maxY = transform.position.y + screenSize.y / 2.0f;
        SpawnInitialAsteroids();

        InvokeRepeating(nameof(SpawnAsteroid), 7f, respawnRate); // Repeatedly spawn asteroids
    }

    /// <summary>
    /// At the start of the level, spawn an initial block of asteroids randomly.
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < maxAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    public IEnumerator DestroyAsteroidCoroutine(Asteroid asteroid)
    {
        asteroid.SetVisible(false);
        LevelManager.Instance.IncrementAsteroidsDestroyed();
        yield return new WaitForSeconds(3f);
        Destroy(asteroid.gameObject);
    }

    /// <summary>
    /// Chooses a random side to spawn the asteroid off screen and initialises the asteroid instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Invalid position is selected.</exception>
    private void SpawnAsteroid()
    {
        var randomSide = (Positions)Random.Range(0, 4);
        Vector2 spawnPosition;

        switch (randomSide)
        {
            case Positions.TOP:
            {
                spawnPosition = new Vector2(Random.Range(minX + xOffset, maxX - xOffset), maxY + 10f);
                break;
            }
            case Positions.LEFT:
            {
                spawnPosition = new Vector2(minX - 10f, Random.Range(minY + yOffset, maxY - yOffset));
                break;
            }
            case Positions.RIGHT:
            {
                spawnPosition = new Vector2(maxX + 10f, Random.Range(minY + yOffset, maxY - yOffset));
                break;
            }
            case Positions.BOTTOM:
            {
                spawnPosition = new Vector2(Random.Range(minX + xOffset, maxX - xOffset), minY - 10f);
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        // Make sure all asteroids are fired towards the centre of the map, but in a random manner.
        float randomDirOffset = Random.Range(-20f, 20f);
        Vector2 targetDirection =
            new Vector2(transform.position.x + randomDirOffset, transform.position.y + randomDirOffset) - spawnPosition;

        GameObject asteroidObject = Instantiate(asteroidPrefab.gameObject, spawnPosition, Quaternion.identity);
        Asteroid newAsteroid = asteroidObject.GetComponent<Asteroid>();
        newAsteroid.SetInitialThrust(targetDirection);
        LargeAsteroids.Add(newAsteroid);
    }
}

public enum Positions
{
    TOP,
    LEFT,
    RIGHT,
    BOTTOM
}