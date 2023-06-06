using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for playing the thruster and laser sound effects for the player.
/// </summary>
public class PlayerSFX : MonoBehaviour
{
    [Header("Player SFX")] [SerializeField]
    private AudioClip thrusterClip;

    [SerializeField] [Range(0f, 1f)] private float thrusterVolume = 1f;
    [SerializeField] private AudioClip laserFireClip;
    [SerializeField] [Range(0f, 1f)] private float laserFireVolume = 1f;

    private AudioSource thrusterSource;
    private AudioSource laserSource;

    // Used to stop conflict with rotation and forward thrust triggering sound twice.
    private bool isThrusting;
    private bool isRotating;

    private void Awake()
    {
        var audioSources = GetComponents<AudioSource>();

        thrusterSource = audioSources[0];
        thrusterSource.clip = thrusterClip;
        thrusterSource.volume = thrusterVolume;
        laserSource = audioSources[1];
    }

    /**
     * Uses PlayerInput system callback to trigger the thruster sound effect if the player
     * presses forwards (currently W / UpArrow).
     */
    public void PlayThrustForwardSFX(InputAction.CallbackContext context)
    {
        isThrusting = context.ReadValue<Vector2>().y != 0; // If Thrust input is forwards.
        PlayThrusterClip();
    }

    /**
     * Uses PlayerInput system callback to trigger the thruster sound effect if the player
     * rotates the ship.
     */
    public void ProcessThrustRotateSFX(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x != 0 && !isThrusting)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }

        PlayThrusterClip();
    }

    // Plays the thruster sound effect if it is not already playing.
    private void PlayThrusterClip()
    {
        if (isThrusting || isRotating)
        {
            if (!thrusterSource.isPlaying)
            {
                thrusterSource.Play();
            }
        }
        else
        {
            thrusterSource.Stop();
        }
    }

    public void PlayFireLaserClip()
    {
        laserSource.PlayOneShot(laserFireClip, laserFireVolume);
    }

    public void StopAllSounds()
    {
        isThrusting = false;
        isRotating = false;
        if (thrusterSource.isPlaying)
        {
            thrusterSource.Stop();
        }
    }
}