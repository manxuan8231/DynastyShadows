using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("FPS Settings")]
    [SerializeField] private Dropdown fpsDropdown;
    private readonly int[] fpsOptions = { 60, 90, 120 };

    void Start()
    {
        LoadVolumeSettings();
        LoadFPSSettings();
    }

    #region Volume
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        }

        SetMusicVolume();
        SetSfxVolume();
    }
    #endregion

    #region FPS
    public void OnFpsDropdownChanged(int index)
    {
        int selectedFps = fpsOptions[index];
        Application.targetFrameRate = selectedFps;
        PlayerPrefs.SetInt("TargetFPS", selectedFps);
    }

    private void LoadFPSSettings()
    {
        int savedFps = PlayerPrefs.GetInt("TargetFPS", 60);
        int index = System.Array.IndexOf(fpsOptions, savedFps);
        if (index == -1) index = 0;

        fpsDropdown.value = index;
        fpsDropdown.onValueChanged.AddListener(OnFpsDropdownChanged);
        Application.targetFrameRate = fpsOptions[index];
    }
    #endregion
}
