using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the main game UI screen that can be seen when in-game.
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerLives;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button pauseButton;
    private PauseMenuUI pauseMenuUI;

    private void Start()
    {
        pauseMenuUI = FindObjectOfType<PauseMenuUI>(true);
    }

    public void UpdateLives(int currentLives)
    {
        switch (currentLives)
        {
            case 1:
                playerLives[0].SetActive(true);
                playerLives[1].SetActive(false);
                playerLives[2].SetActive(false);
                break;
            case 2:
                playerLives[0].SetActive(true);
                playerLives[1].SetActive(true);
                playerLives[2].SetActive(false);
                break;
            case 3:
                playerLives[0].SetActive(true);
                playerLives[1].SetActive(true);
                playerLives[2].SetActive(true);
                break;
            default:
                Debug.Log("Unexpected amount of lives");
                break;
        }
    }

    public void ResetLives()
    {
        playerLives[0].SetActive(true);
        playerLives[1].SetActive(true);
        playerLives[2].SetActive(true);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void PauseGame()
    {
       // Time.timeScale = 0;
        pauseMenuUI.LoadPauseMenu();
        pauseButton.interactable = false;
    }
}