using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : AudioManager {

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME,0.3f);
        SetPlayerPref();
    }

    private void SetPlayerPref() {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public override void ChangeVolume(float volume) {
        base.ChangeVolume(volume);
        SetPlayerPref();        
    }
    
}
