using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsNavigation : MonoBehaviour
{
    public Action<OptionScreen> clicked;


    public void VideoClicked() => clicked?.Invoke(OptionScreen.Video);
    public void AudioClicked() => clicked?.Invoke(OptionScreen.Audio);
    public void GameplayClicked() => clicked?.Invoke(OptionScreen.Game);
    public void ControlsClicked() => clicked?.Invoke(OptionScreen.Controls);
}
