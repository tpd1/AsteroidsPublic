using UnityEngine;

/// <summary>
/// Singleton class that manages general gameplay aspects of a level
/// This is edited to determine the level's difficulty and asteroid speed.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private float asteroidMinForce = 40f;
    [SerializeField] private float asteroidMaxForce = 80f;
    [SerializeField] private int currentLevelIndex;
    private GameUIManager gameUI;
    private PlayerProfile activeProfile;
    private ScriptableObject[] shipObjects;
    private GameOverUI gameOverUI;
    private LevelCompleteUI levelCompleteUI;
    private int AsteroidsDestroyed { get; set; }
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        gameUI = FindObjectOfType<GameUIManager>();
        gameOverUI = FindObjectOfType<GameOverUI>(true);
        levelCompleteUI = FindObjectOfType<LevelCompleteUI>(true);
        shipObjects = ScriptableObjectManager.Instance.ShipScriptableObjs;
        activeProfile = ProfileManager.Instance.GetActiveProfile();
        SpawnPlayer();
    }

    /// <summary>
    /// Increment the number of asteroids destroyed and check for the "100 asteroids destroyed"
    /// achievement.
    /// </summary>
    public void IncrementAsteroidsDestroyed()
    {
        AsteroidsDestroyed++;
        activeProfile.asteroidsDestroyed++;
        if (activeProfile.asteroidsDestroyed == 100)
        {
            AchievementService.Instance.UnlockAchievement(1);
        }
    }

    public float GetAsteroidMinForce()
    {
        return asteroidMinForce;
    }

    public float GetAsteroidMaxForce()
    {
        return asteroidMaxForce;
    }

    /// <summary>
    /// Initiate the player with the current profile's active ship.
    /// </summary>
    private void SpawnPlayer()
    {
        var ship = (Ship)shipObjects[activeProfile.currentShip];
        Instantiate(ship.shipPrefab, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// When the game over state is reached, display the game over UI and save the profile.
    /// </summary>
    public void GameOver()
    {
        StopAllCoroutines();
        gameUI.gameObject.SetActive(false);
        var creditsGained = ScoreController.Instance.CreditsGained;
        activeProfile.credits += creditsGained;
        ProfileManager.Instance.SaveProfiles();
        gameOverUI.LoadLevelCompleteMenu(creditsGained);
    }

    /// <summary>
    /// After successfully completing the level, shown the completion UI with points gained.
    /// Save the current profile's stats.
    /// </summary>
    public void EndLevel()
    {
        StopAllCoroutines();
        gameUI.gameObject.SetActive(false);

        int numOfLevels = activeProfile.levelsCompleted.Length;

        if (currentLevelIndex + 1 < numOfLevels)
        {
            activeProfile.levelsCompleted[currentLevelIndex + 1] = true;
        }

        var creditsGained = ScoreController.Instance.CreditsGained;
        int score = ScoreController.Instance.Score;
        if (score > activeProfile.level1HighScore)
        {
            activeProfile.level1HighScore = score;
        }

        activeProfile.credits += creditsGained;
        ProfileManager.Instance.SaveProfiles();
        levelCompleteUI.LoadLevelCompleteMenu(score, activeProfile.level1HighScore, creditsGained);
    }
}