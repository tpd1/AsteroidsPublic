/// <summary>
/// Interface for implementing the service locator pattern for achievement unlocking.
/// </summary>
public interface IAchievements
{
    void UnlockAchievement(int id);
    bool IsUnlocked(int id);
}


