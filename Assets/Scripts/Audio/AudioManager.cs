using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioLibrary audioLibrary;
    [SerializeField] private AudioSource musicSourcePrefab;
    [SerializeField] private AudioSource sfxSourcePrefab;

    private AudioSource musicSource;
    private readonly Dictionary<string, AudioSource> activeSfx = new Dictionary<string, AudioSource>();

    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        musicSource = Instantiate(musicSourcePrefab, transform);

        bgmVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }
    public void PlayMusic(string musicId, bool loop = true)
    {
        var data = audioLibrary.GetClip(musicId, true);
        if (data == null) return;

        musicSource.clip = data.Clip;
        musicSource.volume = data.Volume * bgmVolume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlaySFX(string sfxId)
    {
        var data = audioLibrary.GetClip(sfxId, false);
        if (data == null) return;

        AudioSource sfxSource = Instantiate(sfxSourcePrefab, transform);
        sfxSource.clip = data.Clip;
        sfxSource.volume = data.Volume * sfxVolume;
        sfxSource.Play();

        Destroy(sfxSource.gameObject, data.Clip.length);
    }

    public void StopSFX(string sfxId)
    {
        if (activeSfx.TryGetValue(sfxId, out var source))
        {
            source.Stop();
            Destroy(source.gameObject);
            activeSfx.Remove(sfxId);
        }
    }
    public void SetBGMVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var kvp in activeSfx)
        {
            if (kvp.Value != null)
                kvp.Value.volume = volume;
        }

        sfxSourcePrefab.volume = volume;
    }

}
