using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUIStep : MonoBehaviour
{
    public event Action<TutorialUIStep> confirmed;

    private TutorialUITask task;

    private void Awake()
    {
        task = GetComponentInChildren<TutorialUITask>();

        var info = GetComponentInChildren<TutorialUIInfo>();
        if(info != null)
        {
            info.confirmed += Confirm;
        }
    }

    public void UpdateProgress(int left)
    {
        if(task != null)
        {
            task.UpdateProgress(left);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        Hide();
        confirmed?.Invoke(this);
    }
}
