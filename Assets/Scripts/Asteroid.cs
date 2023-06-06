using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Controls the functionality of a single asteroid
/// </summary>
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float maxTorque = 100f;
    [SerializeField] private Sprite[] sprites; // selection of random shaped sprites
    [SerializeField] private Asteroid nextAsteroid;
    [SerializeField] private int numOfSplitAsteroids = 2;
    [SerializeField] private ParticleSystem splitEffect;
    private bool isInitialising;
    private ScreenBounds screenBounds;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D asteroidCollider;
    private AsteroidSpawner asteroidSpawner;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        asteroidCollider = GetComponent<Collider2D>();
        screenBounds = FindObjectOfType<ScreenBounds>();
    }

    private void Start()
    {
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        // Pick random sprite from array
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
        isInitialising = true;
    }

    /// <summary>
    /// Pick a random force to initialise the asteroid with.
    /// </summary>
    /// <param name="direction">Direction to fire the asteroid</param>
    public void SetInitialThrust(Vector2 direction)
    {
        rb.AddForce(direction * (Random.Range(LevelManager.Instance.GetAsteroidMinForce(),
            LevelManager.Instance.GetAsteroidMaxForce())));
        rb.AddTorque(Random.Range(-maxTorque, maxTorque));
    }


    private void Update()
    {
        if (!screenBounds.IsOutOfBounds(transform.position, true) && isInitialising)
        {
            isInitialising = false;
        }

        if (screenBounds.IsOutOfBounds(transform.position, true) && !isInitialising)
        {
            transform.position = screenBounds.CalculateNewPosition(transform.position);
        }
    }

    /// <summary>
    /// When the asteroid is destroyed, it is split into smaller asteroids. If the current asteroid
    /// is already a small asteroid, destroy it.
    /// </summary>
    public void Split()
    {
        ParticleSystem instance = Instantiate(splitEffect, transform.position,
            Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration
                                     + instance.main.startLifetime.constantMax);
        if (nextAsteroid != null)
        {
            for (int i = 0; i < numOfSplitAsteroids; i++)
            {
                var randomX = Random.Range(-1f, 1f);
                var randomY = Random.Range(-1f, 1f);
                var offsetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
                var newAsteroid = Instantiate(nextAsteroid, offsetPosition, transform.rotation);
                Vector2 randomDirection = Random.insideUnitCircle.normalized * 50f;
                newAsteroid.SetInitialThrust(randomDirection);
                asteroidSpawner.SmallAsteroids.Add(newAsteroid);
            }

            if (asteroidSpawner.LargeAsteroids.Contains(this))
            {
                asteroidSpawner.LargeAsteroids.Remove(this);
            }
            StartCoroutine(asteroidSpawner.DestroyAsteroidCoroutine(this));
        }
        else
        {
            // Small asteroid
            if (asteroidSpawner.SmallAsteroids.Contains(this))
            {
                asteroidSpawner.SmallAsteroids.Remove(this);
            }
            StartCoroutine(asteroidSpawner.DestroyAsteroidCoroutine(this));
        }
    }
    
    public void SetVisible(bool isVisible)
    {
        sr.enabled = isVisible;
        asteroidCollider.enabled = isVisible;
    }
    
}