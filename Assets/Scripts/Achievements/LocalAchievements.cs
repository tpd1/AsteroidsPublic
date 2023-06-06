using UnityEngine;

/// <summary>
/// Concrete implementation of an achievement class.
/// Updates the players saved data and displays the achievement notification.
/// </summary>
public class LocalAchievements : MonoBehaviour, IAchievements
{
    private PlayerProfile activeProfile;
    private ScriptableObject[] scriptableObjects;
    private AchievementPopupUI achievementPopupUI;

    private void Start()
    {
        activeProfile = ProfileManager.Instance.GetActiveProfile();
        achievementPopupUI = FindObjectOfType<AchievementPopupUI>();
        scriptableObjects = ScriptableObjectManager.Instance.AchievementScriptableObjs;
    }

    /// <summary>
    /// Processes the unlocking of a single achievement. Displays the UI and
    /// serialises player data.
    /// </summary>
    /// <param name="index">Index of the achievement to unlock</param>
    public void UnlockAchievement(int index)
    {
        achievementPopupUI = FindObjectOfType<AchievementPopupUI>();
        bool[] unlockedAchievements = activeProfile.achievementsUnlocked;
        if (index >= unlockedAchievements.Length) return;

        if (!IsUnlocked(index))
        {
            unlockedAchievements[index] = true;
            achievementPopupUI.DisplayAchievement((Achievement)scriptableObjects[index]);
            ProfileManager.Instance.SaveProfiles();
        }
    }
    
    public bool IsUnlocked(int index)
    {
        bool[] unlockedAchievements = activeProfile.achievementsUnlocked;
        return index < unlockedAchievements.Length && unlockedAchievements[index];
    }
}



