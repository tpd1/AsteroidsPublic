using UnityEngine;

/// <summary>
/// Data class to make it easy to store position data for each rewind object.
/// </summary>
public class RewindVariables
{
    public Vector2 Velocity { get; }
    public float AngularVelocity { get; }
    public Vector2 Position { get; }
    public Quaternion Rotation { get; }
    public bool IsActive { get; }

    public RewindVariables(Vector2 position, Quaternion rotation, bool isActive, Vector2 velocity,
        float angularVelocity)
    {
        Position = position;
        Rotation = rotation;
        IsActive = isActive;
        Velocity = velocity;
        AngularVelocity = angularVelocity;
    }
}