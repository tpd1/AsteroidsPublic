using UnityEngine;

/// <summary>
/// Singleton class for playing explosion sound effects.
/// Handle it here because explosions cause side effects that affect audio when destroying objects.
/// </summary>
public class GlobalSFX : MonoBehaviour
{
    public static GlobalSFX Instance { get; private set; }

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
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}