using UnityEngine;
/// <summary>
/// Achievements serialisable object data class.
/// </summary>
[CreateAssetMenu (fileName = "New Achievement",
    menuName = "Scriptable Objects/Achievement")]
public class Achievement : ScriptableObject
{
    public int id;
    public string achievementName;
    public string description;
    public Sprite thumbnailImage;
}



