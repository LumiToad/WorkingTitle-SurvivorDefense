using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum OptionScreen
{
    Audio,
    Video,
    Game,
    Controls
}

public class OptionsScreenNew : MonoBehaviour
{
    private new AudioSettings audio;
    private VideoSettings video;
    private GameSettings game;

    private PlayerInputActions actions;

    [SerializeField, ShowInInspector, Required]
    private Button videoSettingsApply;

    public void SetUp()
    {
        audio = GetComponentInChildren<AudioSettings>(true);
        video = GetComponentInChildren<VideoSettings>(true);
        game = GetComponentInChildren<GameSettings>(true);

        actions = new PlayerInputActions();
        actions.Navigation.Enable();
        actions.Navigation.Pause.Enable();

        audio.SetUp();
        video.SetUp();
    }


    private void OnEnable()
    {
        GetComponentInChildren<SettingsNavigation>().clicked += ShowScreen;
    }

    private void OnDisable()
    {
        GetComponentInChildren<SettingsNavigation>().clicked -= ShowScreen;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ShowScreen(OptionScreen.Audio);
    }

    public void Hide()
    {
        OnCancelClicked();
        gameObject.SetActive(false);
    }

    private void OnCancelClicked()
    {
        audio.OnCancelClicked();
        video.OnCancelClicked();
        game.OnCancelClicked();
    }

    private void Update()
    {
        if (actions.Navigation.Pause.WasPressedThisFrame())
        {
            Hide();
        }
    }

    public void ShowScreen(OptionScreen option)
    {
        audio.gameObject.SetActive(false);
        video.gameObject.SetActive(false);
        game.gameObject.SetActive(false);
        videoSettingsApply.gameObject.SetActive(false);

        switch (option)
        {
            case OptionScreen.Audio:
                audio.gameObject.SetActive(true);
                break;
            case OptionScreen.Video:
                video.gameObject.SetActive(true);
                videoSettingsApply.gameObject.SetActive(true);
                break;
            case OptionScreen.Game:
                game.gameObject.SetActive(true);
                break;
        }
    }
}
