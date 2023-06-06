using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the level complete UI canvas.
/// </summary>
public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }
    
    public void LoadLevelCompleteMenu(int score, int highScore, int credits)
    {
        gameObject.SetActive(true);
        StartCoroutine(UpdateUI(score, highScore, credits));
    }
    

    private IEnumerator UpdateUI(int score, int highScore, int credits)
    {
        yield return new WaitForSeconds(2* transitionTime);
        Time.timeScale = 0;
        gameObject.SetActive(true);
        scoreText.text = $"Your score: {score}";
        highScoreText.text = $"High Score: {highScore}";
        creditsText.text = $"You have earned {credits} credits";
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
