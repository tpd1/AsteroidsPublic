using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the Profile selection UI scene.
/// </summary>
public class ProfileSelectUI : MonoBehaviour
{
    [SerializeField] private List<Toggle> profileToggles;
    [SerializeField] private List<Button> clearButtons;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI levelsText;
    [SerializeField] private TextMeshProUGUI achievementsText;
    [SerializeField] private TextMeshProUGUI shipsText;
    [SerializeField] private float transitionTime = 1f;
    private PlayerProfile activeProfile;
    private Animator transition;
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        activeProfile = ProfileManager.Instance.GetActiveProfile();
        transition = GetComponent<Animator>();
        UpdateUIElements();

        for (int i = 0; i < profileToggles.Count; i++)
        {
            int currentIndex = i; // Capture the current index in a local variable
            profileToggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ProcessToggleChange(currentIndex);
                }
            });
        }
    }

    private void UpdateUIElements()
    {
        UpdateTextFields();
        UpdateToggleUI();
    }

    /// <summary>
    /// Updates the toggle group to shown the active profile as highlighted.
    /// </summary>
    private void UpdateToggleUI()
    {
        int buttonIndex = ProfileManager.Instance.ActiveProfileIndex;
        profileToggles[buttonIndex].isOn = true;
        for (int i = 0; i < clearButtons.Count; i++)
        {
            clearButtons[i].interactable = (i == buttonIndex);
        }
    }

    /// <summary>
    /// Clears the stats for the currently active profile.
    /// As this is only visible for the current profile button, we don't need a reference to the
    /// index of the profile.
    /// </summary>
    public void ResetProfileStats()
    {
        activeProfile.credits = 0;
        activeProfile.levelsCompleted[0] = true;
        activeProfile.levelsCompleted[1] = false;
        activeProfile.levelsCompleted[2] = false;
        Array.Fill(activeProfile.achievementsUnlocked, false);
        activeProfile.asteroidsDestroyed = 0;
        activeProfile.currentShip = 0;
        activeProfile.level1HighScore = 0;
        activeProfile.level2HighScore = 0;
        activeProfile.level3HiScore = 0;
        Array.Fill(activeProfile.shipsUnlocked, false);
        activeProfile.shipsUnlocked[0] = true;

        UpdateUIElements();
    }

    /// <summary>
    /// Updates the profile's stats on screen.
    /// </summary>
    private void UpdateTextFields()
    {
        var userCredits = activeProfile.credits;
        creditsText.text = $"Available Credits: \n {userCredits}";

        var allLevels = activeProfile.levelsCompleted;
        var numUnlocked = allLevels.Count(c => c);
        levelsText.text = $"Levels Unlocked: {numUnlocked}/{allLevels.Length}";

        var allShips = activeProfile.shipsUnlocked;
        var shipsUnlocked = allShips.Count(c => c);
        shipsText.text = $"Ships Unlocked: {shipsUnlocked}/{allShips.Length}";

        var allAchievements = activeProfile.achievementsUnlocked;
        var achUnlocked = allAchievements.Count(c => c);
        achievementsText.text = $"Achievements Unlocked: {achUnlocked}/{allAchievements.Length}";
    }


    private void SetActiveProfile(int index)
    {
        ProfileManager.Instance.SetActiveProfile(index);
        activeProfile = ProfileManager.Instance.GetActiveProfile();
    }

    /// <summary>
    /// Handles the click when a user selects a new profile.
    /// </summary>
    /// <param name="index"></param>
    public void ProcessToggleChange(int index)
    {
        SetActiveProfile(index);
        UpdateUIElements();
    }

    public void HomeButtonClicked()
    {
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }


    /// <summary>
    /// Coroutine that loads a level after the animation transition has finished.
    /// </summary>
    /// <param name="sceneName">Scene to load</param>
    /// <returns>Coroutine enumerator</returns>
    private IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger(Start1);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}