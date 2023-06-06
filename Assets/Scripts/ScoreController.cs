using UnityEngine;

/// <summary>
/// Handles the updating of player scores in-game.
/// </summary>
public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance { get; private set; }

    private int score;
    private int creditsGained;
    private GameUIManager gameUIManager;

    public int CreditsGained => creditsGained;
    public int Score => score;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();
        gameUIManager.UpdateScoreText(score);
    }

    public void ChangeScore(int val)
    {
        score += val;
        Mathf.Clamp(score, 0, 99999);
        gameUIManager.UpdateScoreText(score);
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void AddCredits(int val)
    {
        creditsGained += val;
    }
    
    
}
