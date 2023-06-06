using UnityEngine;
/// <summary>
/// Variation of the standard Health class designed to work just for the enemy UFOs.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 50;
    [SerializeField] private int score = 50;
    [SerializeField] private int credits = 1;
    private int currentHealth;
    private ScoreController scoreController;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] [Range(0f, 1f)] private float explosionVolume = 1f;
    
    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer otherDamageDealer = other.GetComponent<DamageDealer>();
        if (otherDamageDealer == null) return;

        if (otherDamageDealer.GetObjectType() == ObjectType.PLAYERPROJECTILE)
        {
            TakeDamage(otherDamageDealer.GetDamage());
        }
        otherDamageDealer.Hit(); // Destroy projectile
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GlobalSFX.Instance.PlayClip(explosionClip, explosionVolume);
            ParticleSystem instance = Instantiate(explosionEffect, transform.position,
                Quaternion.identity);
            instance.Play();
            Destroy(instance.gameObject, instance.main.duration
                                         + instance.main.startLifetime.constantMax);
            
            scoreController.ChangeScore(score);
            scoreController.AddCredits(credits);
            AchievementService.Instance.UnlockAchievement(0);
            currentHealth = 0;
            gameObject.SetActive(false);
        }
    }
    
}