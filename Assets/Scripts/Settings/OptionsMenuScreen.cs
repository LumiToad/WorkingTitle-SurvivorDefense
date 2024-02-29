using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class OptionsMenuScreen : MonoBehaviour
{
    public const string SETTINGS = "SETTINGS";
    public const string AUDIO = "AUDIO";
    public const string VIDEO = "VIDEO";
    public const string GAME = "GAME";

    private SettingsMenu settingsMenu;
    private AudioSettings audioSettings;
    private VideoSettings videoSettings;
    private GameSettings gameSettings;

    [SerializeField, Required]
    private Image background;

    private static OptionsMenuScreen instance;

    public static bool isGamePaused = false;

    private void Awake()
    {
        instance = this;
        settingsMenu = GetComponentInChildren<SettingsMenu>(true);
        audioSettings = GetComponentInChildren<AudioSettings>(true);
        videoSettings = GetComponentInChildren<VideoSettings>(true);
        gameSettings = GetComponentInChildren<GameSettings>(true);
    }

    private void Start()
    {
        DisableAllMenus();
        background.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            CallPauseMenuInternal(isGamePaused);
        }
    }

    public static void CallMenu(bool value)
    {
        instance.CallMenuInternal(value);
    }

    public void CallMenuInternal(bool value)
    {
        ToggleMenu(settingsMenu, value);
    }

    public static void CallPauseMenu(bool value)
    {
        instance.CallPauseMenuInternal(value);
    }

    private void CallPauseMenuInternal(bool value)
    {
        background.gameObject.SetActive(value);
        ToggleMenu(settingsMenu, value);
        SetGameTimePaused(value);
    }

    public static void BackToSettingsMenu(ISettings settings)
    {
        instance.ToggleMenu(settings, false);
        instance.ToggleMenu(instance.settingsMenu, true);
    }

    public static void ToggleMenu(string settingsByString, bool value)
    {
        ISettings settings = instance.settingsMenu;

        switch (settingsByString) 
        {
            default:
                break;
            case SETTINGS:
                settings = instance.settingsMenu;
                break;
            case AUDIO:
                settings = instance.audioSettings;
                break;
            case VIDEO:
                settings = instance.videoSettings;
                break;
            case GAME:
                settings = instance.gameSettings;
                break;
        }
        
        instance.ToggleMenu(settings, value);
    }

    public void ToggleMenu(ISettings settings, bool value)
    {
        DisableAllMenus();
        //settings.gameObject.SetActive(value);
    }

    private void DisableAllMenus()
    {
        settingsMenu.gameObject.SetActive(false);
        audioSettings.gameObject.SetActive(false);
        videoSettings.gameObject.SetActive(false);
        gameSettings.gameObject.SetActive(false);
    }

    public void SetGameTimePaused(bool value)
    {
        if (value) 
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
