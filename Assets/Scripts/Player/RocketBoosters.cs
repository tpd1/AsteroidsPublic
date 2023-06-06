using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for displaying and hiding the correct booster sprites when the player moves.
/// </summary>
public class RocketBoosters : MonoBehaviour
{
    [SerializeField] private GameObject leftBooster;
    [SerializeField] private GameObject rightBooster;

    private bool isThrusting; // Stops conflict between rotating single boosters & both.

    private void Awake()
    {
        isThrusting = false;
        SetBoosters(false, false);
    }

    /**
     * On forward/backwards thrust input from player, sets the booster sprites to active/disabled.
     */
    public void ProcessBoosterMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y > 0)
        {
            isThrusting = true;
            SetBoosters(true, true);
        }
        else
        {
            isThrusting = false;
            SetBoosters(false, false);
        }
    }

    /**
     * On rotation input from player, sets the left & right booster sprites based on rotation direction.
     */
    public void ProcessBoosterRotate(InputAction.CallbackContext context)
    {
        if (isThrusting) return;

        if (context.ReadValue<Vector2>().x > 0 && !isThrusting)
        {
            SetBoosters(true, false);
        }
        else if (context.ReadValue<Vector2>().x < 0 && !isThrusting)
        {
            SetBoosters(false, true);
        }
        else
        {
            SetBoosters(false, false);
        }
    }

    // Helper method to avoid repeating code.
    private void SetBoosters(bool left, bool right)
    {
        leftBooster.SetActive(left);
        rightBooster.SetActive(right);
    }
}