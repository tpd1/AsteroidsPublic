using UnityEngine;

/**
 * Written following the Youtube tutorial by Sunny Valley Studio:
 * https://www.youtube.com/watch?v=1a9ag16PeFw
 *
 * Handles the player moving off screen, providing a screen wrap for player and asteroids.
 */

[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBounds : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private const float asteroidOffset = 3f;

    [SerializeField] private float resetOffset = 0.2f;
    
    public static ScreenBounds Instance { get; private set; }
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
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }


    private void Start()
    {
        transform.position = Vector3.zero;
        boxCollider.size = GetBoundsSize();
    }


    /// <summary>
    /// Calculate the size of the play area based on the main camera.
    /// </summary>
    /// <returns>The active play area</returns>
    public Vector2 GetBoundsSize()
    {
        float screenHeight = mainCamera.orthographicSize * 2;
        Vector2 colliderSize = new Vector2(screenHeight * mainCamera.aspect, screenHeight);
        return colliderSize;
    }
    
    /// <summary>
    /// Determines whether a position is outside the bounds of the visible screen area.
    /// </summary>
    /// <param name="position">Position vector to be checked.</param>
    /// <param name="isAsteroid">Whether the object is an asteroid or not.</param>
    /// <returns></returns>
    public bool IsOutOfBounds(Vector3 position, bool isAsteroid=false)
    {
        if (!isAsteroid)
        {
            return Mathf.Abs(position.x) > Mathf.Abs(boxCollider.bounds.min.x) ||
                   Mathf.Abs(position.y) > Mathf.Abs(boxCollider.bounds.min.y);  
        }
        
        // If it is an asteroid, apply an offset so visually it lines up with the size of the asteroid.
        return Mathf.Abs(position.x) > Mathf.Abs(boxCollider.bounds.min.x - asteroidOffset) ||
               Mathf.Abs(position.y) > Mathf.Abs(boxCollider.bounds.min.y - asteroidOffset);

    }

    /// <summary>
    /// Returns a new position for the game object. Wraps the object to the other side of the screen.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 CalculateNewPosition(Vector2 position)
    {
        // Check if the position is outside screen bounds.
        bool outOfBoundsX = Mathf.Abs(position.x) > Mathf.Abs(boxCollider.bounds.min.x) - 1;
        bool outOfBoundsY = Mathf.Abs(position.y) > Mathf.Abs(boxCollider.bounds.min.y) - 1;

        //Because we used Abs above, we need sign.
        Vector2 sign = new(Mathf.Sign(position.x), Mathf.Sign(position.y));

        // If Both out of bounds (Corner positions).
        if (outOfBoundsX && outOfBoundsY)
        {
            return Vector2.Scale(position, Vector2.one * -1) +
                   Vector2.Scale(new Vector2(resetOffset, resetOffset), sign);
        }
        // Out of bounds only in X.
        if (outOfBoundsX)
        {
            return new Vector2(position.x * -1, position.y) +
                   new Vector2(resetOffset * sign.x, resetOffset);
        }
        // Out of bounds only in Y.
        if (outOfBoundsY)
        {
            return new Vector2(position.x, position.y * -1) +
                   new Vector2( resetOffset, resetOffset * sign.y);

        }
        // Not out of bounds.
        return position;

    }
}