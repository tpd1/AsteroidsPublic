using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Shooting component of the player's ship. Handles the firing input.
/// </summary>
public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 50f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private float projectileFireRate = 0.3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private ObjectType objectType;
    [SerializeField] private PlayerSFX playerSfx;
    private Coroutine firingCoroutine;
    private bool isFiring;

    private void Start()
    {
        if (objectType == ObjectType.UFO)
        {
            isFiring = true;
        }
    }

    private void Update()
    {
        // Check if the cursor is over a UI element to avoid firing when clicking on buttons.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Fire();
        }
    }


    /// <summary>
    /// Begins the firing coroutine. Allows the player to hold down the firing key for an automatic
    /// style firing rate.
    /// </summary>
    private void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireMultiple());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine that fires the weapon until it is made to stop. Initiates a new projectile then
    /// destroys it after it has gone off screen.
    /// </summary>
    /// <returns>Coroutine enumerator</returns>
    private IEnumerator FireMultiple()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            DamageDealer dd = projectile.GetComponent<DamageDealer>();

            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
                playerSfx.PlayFireLaserClip();
            }

            if (dd != null)
            {
                dd.SetDamage(damage);
            }

            Destroy(projectile, projectileLifetime);
            yield return new WaitForSeconds(projectileFireRate);
        }
    }


    /// <summary>
    /// Processes the actual input from the PlayerInput system when fire is pressed.
    /// </summary>
    /// <param name="context">Action input from the PlayerInput system</param>
    public void ProcessFireInput(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
    }
}