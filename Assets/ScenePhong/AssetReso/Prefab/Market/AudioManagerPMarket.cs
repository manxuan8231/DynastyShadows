using System.Collections.Generic;
using UnityEngine;

public class AudioManagerPMarket : MonoBehaviour
{
    public static AudioManagerPMarket Instance;

    [System.Serializable]
    public class ZoneMusic
    {
        public string zoneName;
        public AudioClip musicClip;
    }

    public List<ZoneMusic> zoneMusics; // Gán trong Inspector
    public AudioSource audioSource;    // Nguồn phát nhạc chính

    private Dictionary<string, AudioClip> musicDict;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitMusicDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitMusicDictionary()
    {
        musicDict = new Dictionary<string, AudioClip>();
        foreach (var zoneMusic in zoneMusics)
        {
            if (!musicDict.ContainsKey(zoneMusic.zoneName))
                musicDict.Add(zoneMusic.zoneName, zoneMusic.musicClip);
        }
    }

    public void PlayZoneMusic(string zoneName)
    {
        if (musicDict.ContainsKey(zoneName))
        {
            AudioClip clip = musicDict[zoneName];
            if (audioSource.clip != clip)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy nhạc cho zone: " + zoneName);
        }
    }
}
