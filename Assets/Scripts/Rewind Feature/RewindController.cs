
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the rewinding of all re-windable objects in the scene.
/// </summary>
public class RewindController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewindText;
    [SerializeField] private TextMeshProUGUI rewindButtonText;
    [SerializeField] private GameObject[] rewindLives;
    [SerializeField] private Button rewindButton;
    public static RewindController Instance { get; private set; }
    private readonly List<ObjectRewind> rewindableObjects = new();
    public static readonly int SecondsToRewind = 3;
    private readonly int rewindCooldown = 3;
    private int rewindsLeft = 3;
    private Color defaultTextColour;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        defaultTextColour = rewindButtonText.color;
    }

    public void AddRewindObject(ObjectRewind obj)
    {
        rewindableObjects.Add(obj);
    }

    public void RemoveRewindObject(ObjectRewind obj)
    {
        rewindableObjects.Remove(obj);
    }

    /// <summary>
    /// Iterate over all rewindable objects and start rewind process
    /// </summary>
    private void BeginRewindAll()
    {
        foreach (ObjectRewind obj in rewindableObjects)
        {
            obj.BeginRewind();
        }
    }

    /// <summary>
    /// Iterate over all rewindable objects and stop rewind process
    /// </summary>
    private void EndRewindAll()
    {
        foreach (ObjectRewind obj in rewindableObjects)
        {
            obj.EndRewind();
        }
    }

    /// <summary>
    /// If there are rewinds left, begin the coroutine to rewind all objects
    /// </summary>
    public void RewindForDuration()
    {
        if (rewindsLeft > 0)
        {
            rewindsLeft--;
            UpdateRewindLives(rewindsLeft);
            StartCoroutine(RewindAllCoroutine());
        }
        else
        {
            SetRewindButton(false);
        }

        if (rewindsLeft == 0)
        {
            AchievementService.Instance.UnlockAchievement(2);
        }
    }
    
    /// <summary>
    /// Coroutine that rewinds all objects for a set duration.
    /// </summary>
    private IEnumerator RewindAllCoroutine()
    {
        // Show "Rewinding" text
        SetRewindingText(true);
        // Make button not interactable & change colour to grey
        SetRewindButton(false);
        // Rewind for duration
        BeginRewindAll();
        yield return new WaitForSeconds(SecondsToRewind);
        EndRewindAll();
        // Hide Rewinding text
        SetRewindingText(false);
        // Cooldown so array can repopulate
        yield return new WaitForSeconds(rewindCooldown);
        if (rewindsLeft > 0) SetRewindButton(true);
    }

    private void SetRewindingText(bool active)
    {
        rewindText.gameObject.SetActive(active);
    }

    private void SetRewindButton(bool active)
    {
        
        rewindButton.interactable = active;
        rewindButtonText.color = active ? defaultTextColour : Color.gray;
    }

    /// <summary>
    /// Updates the rewind UI
    /// </summary>
    /// <param name="currentLives">Number of rewinds left</param>
    private void UpdateRewindLives(int currentLives)
    {
        switch (currentLives)
        {
            case 0:
                rewindLives[0].SetActive(false);
                rewindLives[1].SetActive(false);
                rewindLives[2].SetActive(false);
                break;
            case 1:
                rewindLives[0].SetActive(true);
                rewindLives[1].SetActive(false);
                rewindLives[2].SetActive(false);
                break;
            case 2:
                rewindLives[0].SetActive(true);
                rewindLives[1].SetActive(true);
                rewindLives[2].SetActive(false);
                break;
            case 3:
                rewindLives[0].SetActive(true);
                rewindLives[1].SetActive(true);
                rewindLives[2].SetActive(true);
                break;
            default:
                Debug.Log("Unexpected amount of lives");
                break;
        }
    }

    

}
