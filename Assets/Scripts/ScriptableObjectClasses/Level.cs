using UnityEngine;

/// <summary>
/// Scriptable object that defines a level.
/// </summary>
[CreateAssetMenu (fileName = "New Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public int difficultyStars;
    public Sprite levelImage;
    public Object sceneToLoad;
}




