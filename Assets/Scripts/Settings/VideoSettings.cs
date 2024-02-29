using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour, ISettings
{
    [SerializeField, ShowInInspector, Required]
    private Slider brightness;

    [SerializeField, ShowInInspector, Required]
    private Dropdown displayMode;

    [SerializeField, ShowInInspector, Required]
    private Dropdown quality;

    [SerializeField, ShowInInspector, Required]
    private Dropdown availableResolutions;

    [SerializeField, ShowInInspector, Required]
    private Toggle vSync;

    [SerializeField, ShowInInspector, Required]
    private ResolutionOkay resOkayScreen;

    [SerializeField, ShowInInspector, Required]
    private Button applyButton;

    private const string VIDEO_SAVEFILE_NAME = "videoSettings";

    private void Update()
    {
        if (resOkayScreen.gameObject.activeSelf) return;
        if (Input.GetKeyUp(KeyCode.Return)) 
        {
            OnConfirmClicked();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnCancelClicked();
        }
    }

    public void SetUp()
    {
        brightness.onValueChanged.AddListener(SetBrightness);
        PopulateResolutionDropdown();
        LoadVideoSettings();
        CurrentSettingsToUI();
        applyButton.gameObject.SetActive(true);
        applyButton.interactable = false;
    }

    private void CurrentResolutionToUI()
    {
        for (int i = 0;i < availableResolutions.options.Count;i++)
        {
            var str = Mathf.RoundToInt(Screen.currentResolution.width) + "X" + Mathf.RoundToInt(Screen.currentResolution.height);
            if (availableResolutions.options[i].text == str)
            {
                availableResolutions.value = i;
                return;
            }
        }
    }

    private void CurrentScreenModeToUI(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.ExclusiveFullScreen: // FullScreen
                displayMode.value = 2;
                break;
            case FullScreenMode.FullScreenWindow: //Borderless
                displayMode.value = 1;
                break;
            case FullScreenMode.Windowed: //Windowed
                displayMode.value = 0;
                break;
        }
    }

    private void PopulateResolutionDropdown()
    {
        List<string> resolutionsAsStrings = new();

        foreach(var res in Screen.resolutions)
        {
            var str = Mathf.RoundToInt(res.width) + "X" + Mathf.RoundToInt(res.height);
            resolutionsAsStrings.Add(str);
        }

        availableResolutions.AddOptions(resolutionsAsStrings);
    }

    private Resolution? GetSelectedResolution(string resolutionAsString)
    {
        foreach (Resolution res in Screen.resolutions)
        {
            var str = Mathf.RoundToInt(res.width) + "X" + Mathf.RoundToInt(res.height);
            if (str == resolutionAsString)
            {
                return res;
            }
        }

        return null;
    }

    private void SetGameResolution(Resolution? res, FullScreenMode mode)
    {
        if (res == null) return;
        Resolution resolution = (Resolution)res;

        Screen.SetResolution(resolution.width, resolution.height, mode, resolution.refreshRateRatio);
    }

    private FullScreenMode GetUserSelectedFullscreenMode()
    {
        FullScreenMode fullScreenMode = Screen.fullScreenMode;

        switch (displayMode.value)
        {
            case 0:
                fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }

        return fullScreenMode;
    }

    private void SetBrightness(float value)
    {
        Brightness.SetBrightness(value);
    }

    private void CurrentVSyncToUI()
    {
        vSync.isOn = (QualitySettings.vSyncCount != 0) ? true : false;
    }

    private void CurrentBrightnessToUI()
    {
        brightness.value = Brightness.CurrentBrightness;
    }

    private void SetVSync()
    {
        QualitySettings.vSyncCount = (vSync.isOn) ? 1 : 0;
    }

    private void CurrentQualitySettingToUI()
    {
        int level = QualitySettings.GetQualityLevel();
        quality.value = level;
    }

    private void SetQualitySetting()
    {
        QualitySettings.SetQualityLevel(quality.value);
    }

    public void ApplySettings(FullScreenMode mode, Resolution? res)
    {
        SetGameResolution(res, mode);
        SetQualitySetting();
        SetVSync();
        SetBrightness(brightness.value);
    }

    private void LoadVideoSettings()
    {
        VideoSettingsDTO dto = new();
        if (SaveFileUtils.TryLoadClassFromJsonFile(ref dto, VIDEO_SAVEFILE_NAME))
        {
            Resolution? res = GetSelectedResolution(dto.resolution);
            if (res == null)
            {
                LoadDefaults();
                return;
            }
            brightness.value = dto.brightness;
            quality.value = dto.quality;
            vSync.isOn = dto.vSync;
            CurrentScreenModeToUI(dto.fullScreenMode);
            
            ApplySettings(GetUserSelectedFullscreenMode(), res);
            return;
        }

        LoadDefaults();
    }

    private void LoadDefaults()
    {
        brightness.value = 1.0f;
        quality.value = 1;
        vSync.isOn = false;

        ApplySettings(FullScreenMode.Windowed, Screen.currentResolution);
        SaveSettings();
    }

    private void CurrentSettingsToUI()
    {
        CurrentResolutionToUI();
        CurrentScreenModeToUI(Screen.fullScreenMode);
        CurrentQualitySettingToUI();
        CurrentVSyncToUI();
        CurrentBrightnessToUI();
    }

    private void SaveSettings()
    {
        VideoSettingsDTO dto = new()
        {
            fullScreenMode = Screen.fullScreenMode,
            resolution = Mathf.RoundToInt(Screen.currentResolution.width) + "X" + Mathf.RoundToInt(Screen.currentResolution.height),
            quality = quality.value,
            vSync = vSync.isOn,
            brightness = brightness.value
        };

        SaveFileUtils.SaveClassToJson(dto, VIDEO_SAVEFILE_NAME);
    }

    public void OnCancelClicked()
    {
        LoadVideoSettings();
        SetBrightness(brightness.value);
        CurrentSettingsToUI();
        applyButton.interactable = false;
    }

    public void OnConfirmClicked()
    {
        ApplySettings(GetUserSelectedFullscreenMode(), GetSelectedResolution(availableResolutions.captionText.text));
        resOkayScreen.gameObject.SetActive(true);
        resOkayScreen.Setup(this);
        applyButton.interactable = false;
    }

    public void OnResolutionConfirmClicked()
    {
        SaveSettings();
        CurrentSettingsToUI();
        applyButton.interactable = false;
    }

    public void OnResolutionCancelClicked()
    {
        StartCoroutine(OnResolutionCancelClickedInternal());
        applyButton.interactable = false;
    }

    private IEnumerator OnResolutionCancelClickedInternal()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        applyButton.interactable = false;
        LoadVideoSettings();
        CurrentSettingsToUI();
    }
}
