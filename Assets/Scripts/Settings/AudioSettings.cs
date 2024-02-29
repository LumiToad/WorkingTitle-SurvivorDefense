using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour, ISettings
{
    private const float SLIDER_MIN = 0.0001f;
    private const float SLIDER_MAX = 1.0f;
    private const float DEFAULT_VALUE = 0.7f;
    private const string AUDIO_SAVEFILE_NAME = "audioSettings";

    [SerializeField, ShowInInspector, Required]
    private AudioMixer audioMixer;

    [SerializeField, ShowInInspector, Required]
    private Slider masterVolumeSlider;

    [SerializeField, ShowInInspector, Required]
    private Slider musicVolumeSlider;

    [SerializeField, ShowInInspector, Required]
    private Slider sfxVolumeSlider;

    public void SetUp()
    {
        SetupVolumeSliders();
        SetDefaultVolume();
    }

    private void SetupVolumeSliders()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        masterVolumeSlider.minValue = SLIDER_MIN;
        masterVolumeSlider.maxValue = SLIDER_MAX;

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        musicVolumeSlider.minValue = SLIDER_MIN;
        musicVolumeSlider.maxValue = SLIDER_MAX;

        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        sfxVolumeSlider.minValue = SLIDER_MIN;
        sfxVolumeSlider.maxValue = SLIDER_MAX;
    }

    private void SetDefaultVolume()
    {
        AudioSettingsDTO audioDTO = new();

        if (SaveFileUtils.TryLoadClassFromJsonFile<AudioSettingsDTO>(ref audioDTO, AUDIO_SAVEFILE_NAME))
        {
            SetMasterVolume(audioDTO.masterVolume);
            SetMusicVolume(audioDTO.musicVolume);
            SetSFXVolume(audioDTO.sfxVolume);
            return;
        }

        SetMasterVolume(DEFAULT_VALUE);
        SetMusicVolume(DEFAULT_VALUE);
        SetSFXVolume(DEFAULT_VALUE);
    }

    private void SetMasterVolume(float value)
    {
        masterVolumeSlider.value = value;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("MasterVol", value);

        OnConfirmClicked();
    }

    private void SetMusicVolume(float value)
    {
        musicVolumeSlider.value = value;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("MusicVol", value);

        OnConfirmClicked();
    }

    private void SetSFXVolume(float value)
    {
        sfxVolumeSlider.value = value;
        value = CalculateDecibel(value);
        audioMixer.SetFloat("SFXVol", value);

        OnConfirmClicked();
    }

    public void OnConfirmClicked()
    {
        SaveAudioSettings();
    }

    public void OnCancelClicked()
    {

    }

    private void SaveAudioSettings()
    {
        AudioSettingsDTO audioDTO = new AudioSettingsDTO()
        {
            masterVolume = masterVolumeSlider.value,
            musicVolume = musicVolumeSlider.value,
            sfxVolume = sfxVolumeSlider.value
        };

        SaveFileUtils.SaveClassToJson<AudioSettingsDTO>(audioDTO, AUDIO_SAVEFILE_NAME);
    }

    private float CalculateDecibel(float value)
    {
        return Mathf.Log10(value) * 20.0f;
    }
}
