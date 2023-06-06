using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the Achievement menu UI scene.
/// </summary>
public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] achievementNames;
    [SerializeField] private TextMeshProUGUI[] achievementDescriptions;
    [SerializeField] private Image[] achievementImgs;
    [SerializeField] private TextMeshProUGUI numUnlockedText;
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color greyedColour;
    [SerializeField] private float transitionTime = 0.3f;
    
    private ScriptableObject[] scriptableObjects;
    private PlayerProfile currentProfile;
    private int index;
    private Animator transition;
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        currentProfile = ProfileManager.Instance.GetActiveProfile();
        transition = GetComponent<Animator>();
        scriptableObjects = ScriptableObjectManager.Instance.AchievementScriptableObjs;
        UpdateTextFields();
    }
    
    /// <summary>
    /// Sets all text fields based on the current profile
    /// </summary>
    private void UpdateTextFields()
    {
        
        for (int i = 0; i < achievementNames.Length; i++)
        {
            var currentAchievement = (Achievement)scriptableObjects[i];
            if (!currentProfile.achievementsUnlocked[i])
            {
                achievementNames[i].color = greyedColour;
                achievementDescriptions[i].color = greyedColour;
                achievementImgs[i].color = greyedColour;
            }
            else
            {
                achievementNames[i].color = defaultColour;
                achievementDescriptions[i].color = defaultColour;
                achievementImgs[i].color = defaultColour;
            }
            
            achievementNames[i].text = currentAchievement.achievementName;
            achievementDescriptions[i].text = currentAchievement.description;
            achievementImgs[i].sprite = currentAchievement.thumbnailImage;
            
        }
        
        var allAchievements = currentProfile.achievementsUnlocked;
        int achUnlocked = allAchievements.Count(c => c);
        numUnlockedText.text = $"Achievements Unlocked: {achUnlocked}/{allAchievements.Length}";
        
    }
    
    /// <summary>
    /// Processes the home button click
    /// </summary>
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
