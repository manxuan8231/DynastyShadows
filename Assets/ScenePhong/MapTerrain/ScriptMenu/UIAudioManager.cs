using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIAudioManager : MonoBehaviour
{
    [Header("Chỉ hoạt động ở các Scene sau")]
    public List<string> enabledScenes;

    [Header("Âm thanh mặc định")]
    public AudioClip defaultHoverSound;
    public AudioClip defaultClickSound;

    [Header("Mixer Group (tuỳ chọn)")]
    public AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    public static UIAudioManager Instance { get; private set; }

    void Awake()
    {
        // Tạo Singleton nếu muốn dùng giữa các scene
        // Nếu không cần Singleton, bạn có thể bỏ đoạn này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        SetupAudioSource();
        SceneManager.sceneLoaded += OnSceneLoaded; // Gắn callback khi scene load
    }

    void SetupAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (enabledScenes.Contains(scene.name))
        {
            RegisterAllButtons();
        }
    }

    void RegisterAllButtons()
    {
        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            if (btn.gameObject.GetComponent<UIButtonSound>() == null)
                btn.gameObject.AddComponent<UIButtonSound>();
        }
    }

    public void PlayHover(AudioClip clip = null)
    {
        if (clip == null) clip = defaultHoverSound;
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    public void PlayClick(AudioClip clip = null)
    {
        if (clip == null) clip = defaultClickSound;
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}
