using UnityEngine;

/// <summary>
/// Dictates the amount of damage dealt when collided with another object.
/// Can be attached to asteroids or projectiles.
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private ObjectType objectType;


    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        if (objectType is ObjectType.PLAYERPROJECTILE or ObjectType.ENEMYPROJECTILE)
        {
            Destroy(gameObject);
        }
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}

public enum ObjectType
{
    ASTEROID,
    UFO,
    PLAYER,
    PLAYERPROJECTILE,
    ENEMYPROJECTILE
}