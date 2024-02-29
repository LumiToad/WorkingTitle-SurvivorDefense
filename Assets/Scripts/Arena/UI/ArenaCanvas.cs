using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCanvas : MonoBehaviour
{
    private ArenaTimeDisplay timeDisplay;

    private void Awake()
    {
        timeDisplay = GetComponentInChildren<ArenaTimeDisplay>();
    }

    public void ShowTime(float time) => timeDisplay.ShowTime(time);

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
