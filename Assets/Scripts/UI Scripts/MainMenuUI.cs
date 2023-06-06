using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu UI.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentProfileText;
    [SerializeField] private float transitionTime = 0.3f;

    private static readonly int Start1 = Animator.StringToHash("Start");
    private Animator transition;

    private void Start()
    {
        Application.targetFrameRate = 60;
        transition = GetComponent<Animator>();
        UpdateProfileText();
    }


    private void UpdateProfileText()
    {
        int currentProfileIndex = ProfileManager.Instance.ActiveProfileIndex;
        currentProfileText.text = $"Signed in: Profile {currentProfileIndex + 1} ";
    }


    public void LoadProfilesScene()
    {
        StartCoroutine(LoadLevel("Scenes/Profile Select"));
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger(Start1);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelSelectScene()
    {
        StartCoroutine(LoadLevel("Scenes/Level Select"));
    }

    public void LoadShipSelectScene()
    {
        StartCoroutine(LoadLevel("Scenes/Ship Select"));
    }

    public void LoadAchievementsScene()
    {
        StartCoroutine(LoadLevel("Scenes/Achievements"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}