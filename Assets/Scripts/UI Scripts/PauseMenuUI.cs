using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the pause menu canvas that can be seen in game.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
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
    
    public void ResumeButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        pauseButton.interactable = true;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }

    public void LoadPauseMenu()
    {
        gameObject.SetActive(true);
        StartCoroutine(nameof(PauseCoroutineStart));
    }
    
    private IEnumerator PauseCoroutineStart()
    {
        yield return new WaitForSeconds(2* transitionTime);
        Time.timeScale = 0;
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
