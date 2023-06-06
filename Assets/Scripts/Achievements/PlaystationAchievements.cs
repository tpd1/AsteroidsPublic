using UnityEngine;

/// <summary>
/// Concrete implementation of an achievement class.
/// Dummy class that just prints the achievement to the console for demonstration purposes.
/// </summary>
public class PlaystationAchievements : MonoBehaviour, IAchievements
{
    private PlayerProfile activeProfile;
    private ScriptableObject[] scriptableObjects;

    private void Start()
    {
        activeProfile = ProfileManager.Instance.GetActiveProfile();
        scriptableObjects = ScriptableObjectManager.Instance.AchievementScriptableObjs;
    }
    
    public void UnlockAchievement(int index)
    {
        bool[] unlockedAchievements = activeProfile.achievementsUnlocked;
        if (index >= unlockedAchievements.Length) return;

        if (unlockedAchievements[index] == false)
        {
            Achievement achievement = (Achievement)scriptableObjects[index];
            Debug.Log($"Playstation Achievement Unlocked: {achievement.achievementName}");

            unlockedAchievements[index] = true;
            ProfileManager.Instance.SaveProfiles();
        }
    }

    public bool IsUnlocked(int index)
    {
        bool[] unlockedAchievements = activeProfile.achievementsUnlocked;
        return index < unlockedAchievements.Length && unlockedAchievements[index];
    }
}