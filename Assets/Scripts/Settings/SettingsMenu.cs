using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour, ISettings
{
    public void OnAudioClicked()
    {
        OptionsMenuScreen.ToggleMenu(OptionsMenuScreen.AUDIO, true);
    }

    public void OnVideoClicked()
    {
        OptionsMenuScreen.ToggleMenu(OptionsMenuScreen.VIDEO, true);
    }

    public void OnGameClicked()
    {
        OptionsMenuScreen.ToggleMenu(OptionsMenuScreen.GAME, true);
    }

    public void OnBackClicked()
    {
        //bad - should override OnCancelClicked()
        OptionsMenuScreen.CallPauseMenu(false);
        OptionsMenuScreen.ToggleMenu(OptionsMenuScreen.SETTINGS, false);
        OptionsMenuScreen.isGamePaused = !OptionsMenuScreen.isGamePaused;
    }

    public void OnMainMenuClicked()
    {
        OptionsMenuScreen.isGamePaused = !OptionsMenuScreen.isGamePaused;
        SceneManager.LoadScene(0);
    }

    public void OnCancelClicked()
    {
        
    }

    public void OnConfirmClicked()
    {
        
    }
}
