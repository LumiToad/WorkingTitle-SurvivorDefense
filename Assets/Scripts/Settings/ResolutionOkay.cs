using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionOkay : MonoBehaviour, ISettings
{
    private int time = 10;
    private bool isTimerStarted = false;

    private VideoSettings videoSettings;

    [SerializeField, Required]
    private TextMeshProUGUI reset;

    private void Update()
    {
        if (!isTimerStarted) return;
        if (Input.GetKeyUp(KeyCode.Return))
        {
            OnConfirmClicked();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnCancelClicked();
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        time--;
        if (time == 0)
        { TimerEnd(); }

        if (isTimerStarted ) 
        {
            StartCoroutine(Timer());
        }
        reset.text = $"Reset: {time}";
    }

    public void Setup(VideoSettings videoSettings)
    {
        StartCoroutine (StartTimerOneFrameLater());
        StartCoroutine(Timer());
        this.videoSettings = videoSettings;
    }

    private IEnumerator StartTimerOneFrameLater()
    {
        yield return new WaitForEndOfFrame();
        isTimerStarted = true;
    }

    private void TimerEnd()
    {
        isTimerStarted = false;
        time = 10;

        OnCancelClicked();
    }

    public void OnConfirmClicked()
    {
        isTimerStarted = false;
        time = 10;
        videoSettings.OnResolutionConfirmClicked();
        gameObject.SetActive(false);
    }

    public void OnCancelClicked()
    {
        videoSettings.OnResolutionCancelClicked();
        gameObject.SetActive(false);
    }
}
