using UnityEngine;
/// <summary>
/// Singleton that stores arrays of all scriptable objects, so they can be accessed across the game.
/// </summary>
public class ScriptableObjectManager : MonoBehaviour
{
    public static ScriptableObjectManager Instance { get; private set; }
    [SerializeField] private ScriptableObject[] shipScriptableObjs;
    [SerializeField] private ScriptableObject[] levelScriptableObjs;
    [SerializeField] private ScriptableObject[] achievementScriptableObjs;

    public ScriptableObject[] ShipScriptableObjs => shipScriptableObjs;
    public ScriptableObject[] AchievementScriptableObjs => achievementScriptableObjs;
    public ScriptableObject[] LevelScriptableObjs => levelScriptableObjs;

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
        DontDestroyOnLoad(gameObject);
    }
}

