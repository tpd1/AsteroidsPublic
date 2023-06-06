using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the ship selection UI scene.
/// </summary>
public class ShipSelectUI : MonoBehaviour
{
    [Header("Text Elements")] [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentCreditsText;

    [Header("Images")] [SerializeField] private Image shipImage;
    [SerializeField] private Image[] speedStars;
    [SerializeField] private Image[] damageStars;
    [SerializeField] private Image[] fireRateStars;
    [SerializeField] private Image lockedImage;

    [Header("Buttons")] [SerializeField] private Button buyButton;
    [SerializeField] private Button selectButton;

    [Header("Other")] [SerializeField] private Color lockedColour;
    [SerializeField] private float transitionTime = 0.3f;

    // Private fields
    private ScriptableObject[] shipObjects;
    private PlayerProfile activeProfile;
    private Ship currentShip;
    private Animator transition;
    private int index;
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        transition = GetComponent<Animator>();
        activeProfile = ProfileManager.Instance.GetActiveProfile();
        shipObjects = ScriptableObjectManager.Instance.ShipScriptableObjs;
        ChangeShip(0);
    }

    /// <summary>
    /// Displays a particular ship on screen.
    /// </summary>
    /// <param name="ship">The ship to be displayed.</param>
    private void DisplayShip(Ship ship)
    {
        shipImage.sprite = ship.shipImage;
        SetShipCost(ship.cost);
        DisplayStats(ship.speed, ship.damage, ship.fireRate);
    }

    private void UpdateUIElements()
    {
        UpdateCreditsText();
        UpdateShipUnlocked();
    }

    /// <summary>
    /// Updates the player's current credits.
    /// </summary>
    private void UpdateCreditsText()
    {
        var userCredits = activeProfile.credits;
        currentCreditsText.text = $"Your Credits: {userCredits}";
    }

    /// <summary>
    /// Updates whether the ship is shown as locked or unlocked.
    /// </summary>
    private void UpdateShipUnlocked()
    {
        // Check if current active profile has ship unlocked
        var unlocked = activeProfile.shipsUnlocked[index];

        if (unlocked)
        {
            buyButton.interactable = false;
            selectButton.interactable = true;
            shipImage.color = Color.white;
            lockedImage.gameObject.SetActive(false);
            statusText.text = "Status: Purchased";
        }
        else
        {
            buyButton.interactable = IsCurrentShipBuyable();
            shipImage.color = lockedColour;
            lockedImage.gameObject.SetActive(true);
            selectButton.interactable = false;
            statusText.text = $"Status: Available";
        }
    }

    /// <summary>
    /// Checks whether the user can afford the current ship.
    /// </summary>
    /// <returns>True if the user can afford the ship, false otherwise.</returns>
    private bool IsCurrentShipBuyable()
    {
        int userCredits = activeProfile.credits;
        int shipCost = currentShip.cost;
        return userCredits >= shipCost;
    }

    /// <summary>
    /// Purchases the ship if the user has credits. Checks for the achievement unlock and
    /// updates the UI to reflect the changes.
    /// </summary>
    public void BuyShip()
    {
        int userCredits = activeProfile.credits;
        int shipCost = currentShip.cost;
        if (userCredits - shipCost >= 0)
        {
            activeProfile.credits -= shipCost;
            activeProfile.shipsUnlocked[index] = true;
            ProfileManager.Instance.SaveProfiles();
            CheckForAchievement();
            UpdateUIElements();
        }
    }

    /// <summary>
    /// Checks whether the achievement for unlocking all ships should be processed.
    /// </summary>
    private void CheckForAchievement()
    {
        // If all ships are unlocked, trigger achievement unlock.
        bool allTrue = activeProfile.shipsUnlocked.All(b => b);

        if (allTrue)
        {
            AchievementService.Instance.UnlockAchievement(3);
        }
    }

    public void SelectShip()
    {
        activeProfile.currentShip = index;
        ProfileManager.Instance.SaveProfiles();
    }

    private void SetShipCost(int cost)
    {
        costText.text = $"Cost: {cost} Credits";
    }

    /// <summary>
    /// Displays the ships stats to the screen
    /// </summary>
    /// <param name="speed">Speed rating of the ship  out of 5</param>
    /// <param name="damage">Damage rating of the ship  out of 5</param>
    /// <param name="fireRate">Fire Rate rating of the ship  out of 5</param>
    private void DisplayStats(int speed, int damage, int fireRate)
    {
        for (int i = 0; i < 5; i++)
        {
            speedStars[i].gameObject.SetActive(i < speed);
            damageStars[i].gameObject.SetActive(i < damage);
            fireRateStars[i].gameObject.SetActive(i < fireRate);
        }
    }

    public void HomeButtonClicked()
    {
        StartCoroutine(LoadLevel("Scenes/Main Menu"));
    }

    /// <summary>
    /// Updates the current ship to the next ship in the scriptable object array
    /// </summary>
    /// <param name="newIndex">index of the ship to show</param>
    public void ChangeShip(int newIndex)
    {
        index += newIndex;
        if (index < 0)
        {
            index = shipObjects.Length - 1;
        }
        else if (index > shipObjects.Length - 1)
        {
            index = 0;
        }

        currentShip = (Ship)shipObjects[index];
        UpdateUIElements();
        DisplayShip(currentShip);
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