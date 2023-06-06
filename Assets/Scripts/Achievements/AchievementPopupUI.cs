using TMPro;
using UnityEngine;

/// <summary>
/// Handles the Notification UI that pops up when a player unlocks an achievement.
/// </summary>
public class AchievementPopupUI : MonoBehaviour
{
    private Animator achievementAnimator;
    [SerializeField] private TextMeshProUGUI achievementName;
    [SerializeField] private TextMeshProUGUI achievementDesc;
    private static readonly int Appear = Animator.StringToHash("Appear");

    private void Awake()
    {
        achievementAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Displays an achievement to the screen
    /// </summary>
    /// <param name="achievement">Achievement to be displayed</param>
    public void DisplayAchievement(Achievement achievement)
    {
        achievementName.text = achievement.achievementName;
        achievementDesc.text = achievement.description;
        achievementAnimator.SetTrigger(Appear);
    }

}
