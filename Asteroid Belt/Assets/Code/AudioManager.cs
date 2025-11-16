using UnityEngine;
using System.Collections.Generic; // For managing multiple AudioSources

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    // Add more AudioSources as needed (e.g., for dialogue, ambiance)

    [Header("Audio Clips")]
    public AudioClip defaultMusic; // Assign a default music clip

    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the AudioManager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Example: Play default music on start
        if (defaultMusic != null && musicSource != null)
        {
            PlayMusic(defaultMusic);
        }
    }

    // Method to add sounds to the library (can be called from other scripts or populated in Inspector)
    public void AddSoundToLibrary(string soundName, AudioClip clip)
    {
        if (!soundLibrary.ContainsKey(soundName))
        {
            soundLibrary.Add(soundName, clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' already exists in the library.");
        }
    }

    // Method to play music
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    // Method to play sound effects
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip); // Play SFX without interrupting current SFX
        }
    }

    // Method to play SFX by name from the sound library
    public void PlaySFX(string soundName)
    {
        if (soundLibrary.TryGetValue(soundName, out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in the sound library.");
        }
    }

    // Other methods for stopping, pausing, setting volume, etc.
    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null) musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = volume;
    }
}