using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the health of a game object such as the player, enemy or asteroid.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 50;
    [SerializeField] private int score = 50;
    [SerializeField] private int credits = 1;
    [SerializeField] private UnityEvent zeroHealthEvent;
    [SerializeField] private ObjectType objectType;
    private int currentHealth;
    private ScoreController scoreController;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }

    /// <summary>
    /// Handles projectile impact with the game object.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer otherDamageDealer = other.GetComponent<DamageDealer>();
        if (otherDamageDealer == null) return;

        ObjectType otherType = otherDamageDealer.GetObjectType();
        if ((objectType == ObjectType.ASTEROID || objectType == ObjectType.UFO) &&
            otherType == ObjectType.PLAYERPROJECTILE
            || (objectType == ObjectType.PLAYER && otherType == ObjectType.ENEMYPROJECTILE)
           )
        {
            TakeDamage(otherDamageDealer.GetDamage());
            otherDamageDealer.Hit(); // Destroy projectile
        }
    }

    /// <summary>
    /// Handles physical impacts such as the player colliding with an asteroid.
    /// </summary>
    /// <param name="other">The collider of the other game object.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        DamageDealer otherDamageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (otherDamageDealer == null) return;

        ObjectType otherType = otherDamageDealer.GetObjectType();
        if (objectType == ObjectType.PLAYER &&
            (otherType == ObjectType.ASTEROID || otherType == ObjectType.ENEMYPROJECTILE))
        {
            TakeDamage(otherDamageDealer.GetDamage());
            otherDamageDealer.Hit();
        }
    }

    /// <summary>
    /// Apply damage. If the game object is not attached to a player, give the player correct points.
    /// </summary>
    /// <param name="damage"></param>
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (objectType != ObjectType.PLAYER)
            {
                scoreController.ChangeScore(score);
                scoreController.AddCredits(credits);
            }

            currentHealth = 0;
            zeroHealthEvent.Invoke();
        }
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
    }
}