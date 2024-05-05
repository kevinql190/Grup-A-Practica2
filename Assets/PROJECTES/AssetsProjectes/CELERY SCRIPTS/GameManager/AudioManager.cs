using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip[] musicSounds, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;
    private List<AudioSource> audioSources;
    public AudioMixer mixer;
    private void Start()
    {
        LoadVolume();
        audioSources = new();
    }
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float value = PlayerPrefs.GetFloat("masterVolume");
            mixer.SetFloat("Master", Mathf.Log10(value) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("masterVolume", 0.5f);
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float value = PlayerPrefs.GetFloat("musicVolume");
            mixer.SetFloat("Music", Mathf.Log10(value) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            float value = PlayerPrefs.GetFloat("sfxVolume");
            mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.5f);
        }
    }
    public void PlaySFXOnce(string name, float volumeScale = 1f)
    {
        PlayOnce(name, sfxSounds, volumeScale);
    }
    public void PlayMusicOnce(string name, float volumeScale = 1f)
    {
        PlayOnce(name, musicSounds, volumeScale);
    }
    public void PlayOnce(string name, AudioClip[] audioArray, float volumeScale = 1f)
    {
        AudioClip clip = FindClipByName(name, audioArray);
        if (clip == null)
            return;
        else sfxSource.PlayOneShot(clip, volumeScale);
    }
    #region Loops
    public void PlaySFXLoop(string clipName, float volumeScale = 1f, float lerpTime = 0f)
    {
        // Find the AudioClip in the audioClips array
        AudioClip clip = FindClipByName(clipName, sfxSounds);
        if (clip == null)
            return;

        // Check if an AudioSource with the given clip already exists
        //if (ListContainsClip(clipName))
        //    return;

        // Create AudioSource component
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        // Set up AudioSource to play the clip on loop
        source.clip = clip;
        source.loop = true;
        if (lerpTime == 0) source.volume = volumeScale;
        else StartCoroutine(AudioSourceSmoothTransition(source, lerpTime, true, volumeScale));
        source.Play();

        // Add the AudioSource to the array
        audioSources.Add(source);
    }

    public void StopLoop(string clipName, float lerpTime = 0f)
    {
        // Find the AudioSource with the given clipName
        AudioSource source = FindAudioSourceByClipName(clipName);

        // If AudioSource is found, stop and remove it
        if (source != null)
        {
            if (lerpTime == 0) Destroy(source);
            else StartCoroutine(AudioSourceSmoothTransition(source, lerpTime));
        }
        else Debug.Log("Couldn't stop audio " + clipName + " because it's not playing");
    }
    public void StopAllLoops(float lerpTime = 0f)
    {
        foreach(AudioSource source in audioSources.ToArray())
        {
            StartCoroutine(AudioSourceSmoothTransition(source, lerpTime));
        }
        audioSources = new();
    }

    // Helper method to check if audioSources array contains an AudioSource with the given clip name
    /*
    private bool ListContainsClip(string clipName)
    {
        foreach (var source in audioSources)
        {
            if (source != null && source.clip != null && source.clip.name == clipName)
                return true;
        }
        return false;
    }
    */
    // Helper method to find AudioSource by clip name
    private AudioSource FindAudioSourceByClipName(string clipName)
    {
        foreach (var source in audioSources)
        {
            if (source != null && source.clip != null && source.clip.name == clipName)
                return source;
        }
        return null;
    }
    #endregion
    // Helper method to find AudioClip in the audioClips array by name
    private AudioClip FindClipByName(string clipName, AudioClip[] audioClips)
    {
        foreach (var clip in audioClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        Debug.Log("Sound " + name + " Not Found");
        return null;
    }
    private IEnumerator AudioSourceSmoothTransition(AudioSource source, float lerpTime, bool isSmoothStart = false, float targetVolume = 1f)
    {
        float t = 0;
        float startVolume = isSmoothStart ? 0f : source.volume;
        float endVolume = isSmoothStart ? targetVolume : 0f;

        while (t < lerpTime && source != null)
        {
            t += Time.unscaledDeltaTime;
            float value = t / lerpTime;
            source.volume = Mathf.SmoothStep(startVolume, endVolume, value);
            yield return null;
        }
        if (!isSmoothStart)
        {
            audioSources.Remove(source);
            Destroy(source);
        }
    }
}
