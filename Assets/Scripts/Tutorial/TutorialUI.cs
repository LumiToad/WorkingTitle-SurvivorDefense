using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialDisplays
{
    welcome,
    activateGenerator,
    weapons,
    destroyEnemies,
    XPItems,
    collectXPItems,
    towers,
    congratulations
}

public class TutorialUI : SerializedMonoBehaviour
{
    public event Action<TutorialDisplays> confirmed;

    [SerializeField]
    private Dictionary<TutorialDisplays, TutorialUIStep> steps;

    private void Awake()
    {
        foreach(var step in steps.Values)
        {
            step.confirmed += OnConfirm;
        }
    }

    public void Show(TutorialDisplays display)
    {
        steps[display].Show();
    }

    public void UpdateProgress(TutorialDisplays display, int left)
    {
        steps[display].UpdateProgress(left);
    }

    public void Hide(TutorialDisplays display)
    {
        steps[display].Hide();
    }

    private void OnConfirm(TutorialUIStep step)
    {
        confirmed?.Invoke(steps.Reverse()[step]);
    }
}
