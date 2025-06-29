using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager instance {  get; private set; }
    private AudioSource musicSource;
    private float volume;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.3f);
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = volume;
    }

    public void ChangeVolume(float volumeValue)
    {
        volume = volumeValue / 100;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume * 100;
    }
}
