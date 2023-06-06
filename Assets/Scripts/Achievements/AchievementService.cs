using UnityEngine;

/// <summary>
/// Public interface between the achievement unlock and UI/data layers.
/// Implementation of the service locator pattern.
/// </summary>
public class AchievementService : MonoBehaviour
{
    private IAchievements achievements;
    public static AchievementService Instance { get; private set; }

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

        achievements = GetComponent<IAchievements>();
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockAchievement(int id)
    {
        achievements.UnlockAchievement(id);
    }

    public bool IsUnlocked(int id)
    {
        return achievements.IsUnlocked(id);
    }
}