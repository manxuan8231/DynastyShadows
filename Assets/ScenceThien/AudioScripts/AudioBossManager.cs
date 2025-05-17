using UnityEngine;
using System.Collections.Generic;
public class AudioBossManager : MonoBehaviour
{
    public AudioBossManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Audio Clips")]
    public List<AudioClip> sfxClips; // List âm thanh SFX (attack, get hit, etc)
    public List<AudioClip> bgmClips; // Nhac nen

    private Dictionary<string, AudioClip> sfxDict;
    private Dictionary<string, AudioClip> bgmDict;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Gán tên -> clip
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
            sfxDict[clip.name] = clip;

        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var clip in bgmClips)
            bgmDict[clip.name] = clip;
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name))
            sfxSource.PlayOneShot(sfxDict[name]);
        else
            Debug.LogWarning("SFX not found: " + name);
    }

    public void PlayBGM(string name)
    {
        if (bgmDict.ContainsKey(name))
        {
            bgmSource.clip = bgmDict[name];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
            Debug.LogWarning("BGM not found: " + name);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}