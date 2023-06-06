using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// General player class responsible for player movement.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxVelocity = 15f;
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] int startingLives = 3;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] [Range(0f, 1f)] private float explosionVolume = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D cl;
    private GameUIManager gameUI;
    private PlayerSFX playerSfx;
    private Health playerHealth;
    private int currentLives;
    private ScoreController scoreController;

    // Gets set on player input (update handled by PlayerInput).
    private float rawThrustInput;
    private float rawRotationInput;


    private void Awake()
    {
        currentLives = startingLives;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<CircleCollider2D>();
        playerHealth = GetComponent<Health>();
    }

    private void Start()
    {
        playerSfx = GetComponentInChildren<PlayerSFX>();
        gameUI = FindObjectOfType<GameUIManager>();
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void Update()
    {
        // Check that ship is in bounds, if not, then wrap to other side of screen.
        if (ScreenBounds.Instance.IsOutOfBounds(transform.position))
        {
            transform.position = ScreenBounds.Instance.CalculateNewPosition(transform.position);
        }
    }

    // Apply thrust and rotation to rigidbody in FixedUpdate.
    private void FixedUpdate()
    {
        ApplyThrust();
        ApplyRotation();
    }

    // Applies forward /backwards force to the rigidbody.
    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector2.up * (rawThrustInput * moveForce));
        Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    // Applies rotation force to the rigidbody.
    private void ApplyRotation()
    {
        rb.AddTorque(-rawRotationInput * rotationSpeed);
    }

    // On player movement input, sets the thrust input (-1, 0 or 1)
    public void ProcessThrustInput(InputAction.CallbackContext context)
    {
        rawThrustInput = context.ReadValue<Vector2>().y;
    }

    // On player rotation input, sets the rotate input (-1, 0 or 1)
    public void ProcessRotateInput(InputAction.CallbackContext context)
    {
        rawRotationInput = context.ReadValue<Vector2>().x;
    }

    // processes what happens when the player's health reaches zero
    public void ProcessZeroHealth()
    {
        playerSfx.StopAllSounds();
        GlobalSFX.Instance.PlayClip(explosionClip, explosionVolume);
        // play explosion particles
        ParticleSystem instance = Instantiate(explosionEffect, transform.position,
            Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration
                                     + instance.main.startLifetime.constantMax);
        
        currentLives--;
        scoreController.ChangeScore(-100);
        if (currentLives > 0)
        {
            // Disable collider and sprite renderer 
            // DONT set game object to not active, causes audio bugs.
            cl.enabled = false;
            sr.enabled = false;
            
            rb.velocity = Vector2.zero;
            playerHealth.ResetHealth();
            gameUI.UpdateLives(currentLives);
            Invoke(nameof(ResetPlayer), 2f);
        }
        else
        {
            LevelManager.Instance.GameOver();
        }
    }

    private void ResetPlayer()
    {
        cl.enabled = true;
        sr.enabled = true;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        playerHealth.ResetHealth();
    }
}