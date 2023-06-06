using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the Level Selection UI scene.
/// </summary>
public class LevelSelectUI : MonoBehaviour
{
    [Header("Text Elements")] [SerializeField]
    private TextMeshProUGUI levelName;

    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Images")] [SerializeField] private Image levelImage;
    [SerializeField] private Image lockedImage;
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite activeStar;
    [SerializeField] private Sprite inactiveStar;
    [SerializeField] private float transitionTime = 0.3f;
    [Header("Buttons")] [SerializeField] private Button playButton;

    private Animator transition;
    private int diffStars;
    private ScriptableObject[] scriptableObjects;
    private PlayerProfile currentProfile;
    private int index;
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        currentProfile = ProfileManager.Instance.GetActiveProfile();
        transition = GetComponent<Animator>();
        scriptableObjects = ScriptableObjectManager.Instance.LevelScriptableObjs;
        ChangeLevel(0);
    }


    /// <summary>
    /// Displays a level to the screen
    /// </summary>
    /// <param name="level">Level data to be shown</param>
    /// <param name="levelIndex">Index of the level to be shown</param>
    private void DisplayLevel(Level level, int levelIndex)
    {
        levelName.text = level.levelName;
        diffStars = level.difficultyStars;
        levelImage.sprite = level.levelImage;
        DisplayStars();
        UpdateLocked(levelIndex);
        UpdateHighScoreText(levelIndex);
    }

    /// <summary>
    /// Determines which scene loads when the play button is clicked.
    /// </summary>
    public void PlayButtonClicked()
    {
        // Had to simplify this because of an issue - built game would not load level.
        string scene = "Scenes/Level1";
        switch (index)
        {
            case 0:
                scene = "Scenes/Level1";
                break;
            case 1:
                scene = "Scenes/Level 2";
                break;
            case 2:
                scene = "Scenes/Level 3";
                break;
        }
        StartCoroutine(LoadLevel(scene));
    }

    private void UpdateHighScoreText(int index)
    {
        int highScore = index switch
        {
            0 => currentProfile.level1HighScore,
            1 => currentProfile.level2HighScore,
            2 => currentProfile.level3HiScore,
            _ => 0
        };
        highScoreText.text = $"High Score: {highScore}";
    }

    /// <summary>
    /// Displays the difficulty rating.
    /// </summary>
    private void DisplayStars()
    {
        switch (diffStars)
        {
            case 1:
                stars[0].sprite = activeStar;
                stars[1].sprite = inactiveStar;
                stars[2].sprite = inactiveStar;
                break;
            case 2:
                stars[0].sprite = activeStar;
                stars[1].sprite = activeStar;
                stars[2].sprite = inactiveStar;
                break;
            case 3:
                stars[0].sprite = activeStar;
                stars[1].sprite = activeStar;
                stars[2].sprite = activeStar;
                break;
        }
    }

    /// <summary>
    /// Updates the locked levels
    /// </summary>
    /// <param name="index">index of the current level</param>
    private void UpdateLocked(int index)
    {
        var levelsUnlocked = currentProfile.levelsCompleted;
        if (levelsUnlocked[index])
        {
            lockedImage.gameObject.SetActive(false);
            playButton.interactable = true;
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
            playButton.interactable = false;
        }
    }

    public void HomeButtonClicked()
    {
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }
    
    public void ChangeLevel(int newIndex)
    {
        index += newIndex;
        if (index < 0)
        {
            index = scriptableObjects.Length - 1;
        }
        else if (index > scriptableObjects.Length - 1)
        {
            index = 0;
        }

        DisplayLevel((Level)scriptableObjects[index], index);
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