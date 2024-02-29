using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<OptionsScreenNew>(true).SetUp();
        GetComponentInChildren<CreditsScreen>(true).SetUp();
        GetComponentInChildren<AchivementUI>(true).SetUp();
    }

    public void StartGame()
    {
        if (FadeScreen.TryFadeOut(1.0f))
        {
            FadeScreen.FadeCompleted += OnFadeOutCompleted;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnFadeOutCompleted()
    {
        FadeScreen.FadeCompleted -= OnFadeOutCompleted;
        SceneManager.LoadScene(1);
    }
}
