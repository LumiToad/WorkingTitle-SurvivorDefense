using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField]
    private OptionsScreenNew settings;
    [SerializeField]
    private AchivementUI achivements;

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Hide()
    {
        if (settings.gameObject.activeInHierarchy)
        {
            settings.Hide();
            return;
        }

        if (achivements.gameObject.activeInHierarchy)
        {
            achivements.Hide();
            return;
        }

        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ContinueClicked() => Hide();
    public void SettingsClicked() => settings.Show();
    public void AchivementsClick() => achivements.Show();
    public void RestartClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void MainMenuClicked()
    {
        if (FadeScreen.TryFadeOut(1.0f))
        {
            FadeScreen.FadeCompleted += LoadMainMenu;
        }
    }

    private void LoadMainMenu()
    {
        FadeScreen.FadeCompleted -= LoadMainMenu;
        SceneManager.LoadScene(0);
    }

    public void SetUp()
    {
        GetComponentInChildren<OptionsScreenNew>(true).SetUp();
    }
}
