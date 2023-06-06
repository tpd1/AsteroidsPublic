using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Singleton class responsible for loading and saving player profiles to disk.
/// </summary>
public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance { get; private set; }
    public int ActiveProfileIndex { get; private set; }
    private const int TotalProfiles = 3;
    private List<PlayerProfile> profiles;

    private void Awake()
    {
        profiles = new List<PlayerProfile>();
        LoadProfiles();
        
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


    /// <summary>
    /// Sets the currently active player profile
    /// </summary>
    /// <param name="index">index of the profile to make active.</param>
    public void SetActiveProfile(int index)
    {
        ActiveProfileIndex = index;
        SaveProfiles();
    }

    public PlayerProfile GetActiveProfile()
    {
        return profiles[ActiveProfileIndex];
    }

    /// <summary>
    /// Loads all profiles from binary files. If no profiles exist, create new ones.
    /// </summary>
    private void LoadProfiles()
    {
        string profilesPath = Application.persistentDataPath + "/profiles.dat";

        if (File.Exists(profilesPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(profilesPath, FileMode.Open);

            profiles = formatter.Deserialize(stream) as List<PlayerProfile>;
            stream.Close();
        }
        else
        {
            for (int i = 0; i < TotalProfiles; i++)
            {
                profiles.Add(new PlayerProfile());
            }
        }
        // Load the active profile
        string currProfilePath = Application.persistentDataPath + "/activeProfile.dat";
        if (File.Exists(currProfilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(currProfilePath, FileMode.Open);

            ActiveProfileIndex = (int)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            ActiveProfileIndex = 0; // If no active profile, just set it as profile 1.
        }

    }

    /// <summary>
    /// Saves all profiles to disk using the BinaryFormatter. 
    /// </summary>
    public void SaveProfiles()
    {
        // Save profile data
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/profiles.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, profiles);
        stream.Close();

        // Save the active profile index
        path = Application.persistentDataPath + "/activeProfile.dat";
        stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, ActiveProfileIndex);
        stream.Close();
    }
}

/// <summary>
/// Player data class, defines what will be saved for each player.
/// </summary>
[Serializable]
public class PlayerProfile
{
    public int credits;
    public int currentShip;
    public int asteroidsDestroyed;
    public int level1HighScore;
    public int level2HighScore;
    public int level3HiScore;

    public bool[] levelsCompleted;
    public bool[] achievementsUnlocked;
    public bool[] shipsUnlocked;


    public PlayerProfile( int credits = 0, int currentShip = 0, int asteroidsDestroyed = 0, int level1HighScore = 0,
        int level2HighScore = 0, int level3HiScore = 0, bool[] levelsCompleted = null,
        bool[] achievementsUnlocked = null, bool[] shipsUnlocked = null)
    {
        this.credits = credits;
        this.currentShip = currentShip;
        this.asteroidsDestroyed = asteroidsDestroyed;
        this.level1HighScore = level1HighScore;
        this.level2HighScore = level2HighScore;
        this.level3HiScore = level3HiScore;
        
        this.levelsCompleted = levelsCompleted ?? new[] { true, false, false };
        this.achievementsUnlocked = achievementsUnlocked ?? new bool[4];
        this.shipsUnlocked = shipsUnlocked ?? new[] { true, false, false, false, false };
    }
}