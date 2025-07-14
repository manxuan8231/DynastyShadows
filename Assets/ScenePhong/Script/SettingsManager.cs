using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("FPS Settings")]
    [SerializeField] private TMP_Dropdown fpsDropdown;
    private readonly int[] fpsOptions = { 60, 90, 120 };

    public static SettingsManager Instance;
    void Start()
    {
        LoadVolumeSettings();
        LoadFPSSettings();
        LoadVSyncSettings();
        LoadResolutionSettings();
        LoadQualitySettings();
        LoadDisplayModeSetting();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Ngăn tạo trùng
        }
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

    #region VSync

    [Header("VSync Settings")]
    [SerializeField] private Toggle vsyncToggle;

    public void OnVSyncToggleChanged(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSyncEnabled", isOn ? 1 : 0);
    }

    private void LoadVSyncSettings()
    {
        bool isVSyncOn = PlayerPrefs.GetInt("VSyncEnabled", 1) == 1; // Mặc định bật
        vsyncToggle.isOn = isVSyncOn;
        QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;
        vsyncToggle.onValueChanged.AddListener(OnVSyncToggleChanged);
    }

    #endregion

    #region Display Mode

    [SerializeField] private TMP_Dropdown displayModeDropdown;

    private readonly string[] displayModes = { "Đầy màn", "Cửa sổ", "không viền" };

    private void LoadDisplayModeSetting()
    {
        int savedIndex = PlayerPrefs.GetInt("DisplayModeIndex", 0);

        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string>(displayModes));

        displayModeDropdown.value = savedIndex;
        displayModeDropdown.RefreshShownValue();

        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);

        ApplyDisplayMode(savedIndex);
    }

    private void OnDisplayModeChanged(int index)
    {
        ApplyDisplayMode(index);
        PlayerPrefs.SetInt("DisplayModeIndex", index);
    }

    private void ApplyDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // Borderless
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
    }

    #endregion

    #region Resolution & Quality

    [Header("Resolution & Quality Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private List<Resolution> uniqueResolutions = new List<Resolution>();
    private Resolution[] resolutions;

    private void LoadResolutionSettings()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();

        HashSet<string> seen = new HashSet<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionText = resolutions[i].width + " x " + resolutions[i].height;

            if (!seen.Contains(resolutionText))
            {
                seen.Add(resolutionText);
                uniqueResolutions.Add(resolutions[i]);
                options.Add(resolutionText);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener((index) =>
        {
            Resolution selectedRes = uniqueResolutions[index];
            Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
        });
    }



    public void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < uniqueResolutions.Count)
        {
            Resolution selected = uniqueResolutions[index];
            Screen.SetResolution(selected.width, selected.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
        }
    }

    private void LoadQualitySettings()
    {
        qualityDropdown.ClearOptions();
        string[] englishNames = QualitySettings.names;

        List<string> vietnameseNames = new List<string>();

        foreach (string name in englishNames)
        {
            switch (name.ToLower())
            {
                case "very low":
                    vietnameseNames.Add("Rất thấp");
                    break;
                case "low":
                    vietnameseNames.Add("Thấp");
                    break;
                case "medium":
                    vietnameseNames.Add("Trung bình");
                    break;
                case "high":
                    vietnameseNames.Add("Cao");
                    break;
                case "very high":
                    vietnameseNames.Add("Rất cao");
                    break;
                case "ultra":
                    vietnameseNames.Add("Cực đại");
                    break;
                default:
                    vietnameseNames.Add(name); // fallback nếu có thêm mức khác
                    break;
            }
        }

        qualityDropdown.AddOptions(vietnameseNames);

        int savedIndex = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedIndex;
        qualityDropdown.RefreshShownValue();

        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        QualitySettings.SetQualityLevel(savedIndex);
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
    }

    #endregion


}
