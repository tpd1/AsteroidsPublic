using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Game Over UI canvas.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private float transitionTime = 0.3f;
    private Animator transition;
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        transition = GetComponent<Animator>();
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }

    private IEnumerator UpdateUI(int credits)
    {
        yield return new WaitForSeconds(2 * transitionTime);
        Time.timeScale = 0;
        gameObject.SetActive(true);
        creditsText.text = $"You have earned {credits} credits";
    }
    
    public void LoadLevelCompleteMenu(int credits)
    {
        gameObject.SetActive(true);
        StartCoroutine(UpdateUI(credits));
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
