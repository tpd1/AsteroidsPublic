using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the enemy weapon firing mechanics through a coroutine.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float minFireInterval = 1f; // minimum time between shots
    [SerializeField] private float maxFireInterval = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private PlayerSFX playerSfx;

    private Coroutine firingCoroutine;
    private bool isFiring;


    private void Start()
    {
        StartCoroutine(FireRandom());
    }

    /// <summary>
    /// Fires a projectile at random intervals in an arc towards the player.
    /// </summary>
    /// <returns>Coroutine enumerator</returns>
    private IEnumerator FireRandom()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            DamageDealer dd = projectile.GetComponent<DamageDealer>();

            if (rb != null)
            {
                float angle = Random.Range(-60f, -120f);

                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad));
                rb.velocity = direction * projectileSpeed;

                if (playerSfx != null) playerSfx.PlayFireLaserClip();
            }

            if (dd != null)
            {
                dd.SetDamage(damage);
            }

            Destroy(projectile, projectileLifetime);
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
        }
    }
}