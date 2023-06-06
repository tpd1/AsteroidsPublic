using UnityEngine;

/// <summary>
/// Scriptable object that defines a player ship.
/// </summary>
[CreateAssetMenu (fileName = "New Ship", menuName = "Scriptable Objects/Ship")]
public class Ship : ScriptableObject
{
    public int cost;
    public int speed;
    public int damage;
    public int fireRate;
    public Sprite shipImage;
    public GameObject shipPrefab;

}