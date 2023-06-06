using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class that handles the playing of menu and in-game music.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip menuMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Plays the menu music on all scenes except the in-game levels.
    /// If a transition is made from UI scene -> UI scene, do not restart the music.
    /// </summary>
    /// <param name="scene">The scene that is loaded.</param>
    /// <param name="mode">Not used.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int buildIndex = scene.buildIndex;

        if (buildIndex is > 0 and < 4) // for levels 1, 2, and 3
        {
            if (audioSource.clip != gameMusic)
            {
                if (!audioSource.enabled)
                    audioSource.enabled = true;

                audioSource.clip = gameMusic;
                audioSource.Play();
            }
        }
        else // for menu scenes
        {
            if (audioSource.clip != menuMusic)
            {
                if (!audioSource.enabled)
                    audioSource.enabled = true;

                audioSource.clip = menuMusic;
                audioSource.Play();
            }
        }
    }

    // Unsubscribe to avoid memory leaks.
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}