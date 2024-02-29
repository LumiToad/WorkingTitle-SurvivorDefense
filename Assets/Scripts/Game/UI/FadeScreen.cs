using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private Image image;

    private Color fadeTo = Color.clear;
    private float fadeDuration;

    private bool isFadeStarted = false;

    public static FadeScreen instance;

    public static Action FadeCompleted;

    private float time = 0.0f;

    public static bool TryFadeIn(float duration) => instance.TryStartFadeInInternal(duration);
    public static bool TryFadeOut(float duration) => instance.TryStartFadeOutInternal(duration);

    private void Awake()
    {
        instance = this;
        image = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        if (!TryStartFadeInInternal(2.0f))
        {
            image.color = Color.clear;
        }
    }

    private bool TryStartFadeInInternal(float duration)
    {
        if (isFadeStarted) return false;

        isFadeStarted = true;
        image.color = Color.black;
        fadeTo = Color.clear;
        fadeDuration = duration;

        return true;
    }

    private bool TryStartFadeOutInternal(float duration)
    {
        if (isFadeStarted) return false;

        isFadeStarted = true;
        image.color = Color.clear;
        fadeTo = Color.black;
        fadeDuration = duration;

        return true;
    }

    private void Update()
    {
        if (!isFadeStarted) return;
        time+= Time.deltaTime;

        if (image.color == fadeTo)
        {
            FadeCompleted?.Invoke();
            isFadeStarted = false;
            time = 0f;
        }

        image.color = GetColorSlow();
    }

    private Color GetColorSlow()
    {
        Color color = image.color;
        float clampedUnscaledDeltaTime = Mathf.Clamp(Time.unscaledDeltaTime, 0.00001f, Time.maximumDeltaTime);
        float factor = 1 / fadeDuration;
        color.a += ((fadeTo.a == 0) ? -factor : factor) * ((Time.timeScale == 1.0f) ? Time.deltaTime : clampedUnscaledDeltaTime);
        color.a = Mathf.Clamp(color.a, -0.0f, 1.0f);
        return color;
    }
}
