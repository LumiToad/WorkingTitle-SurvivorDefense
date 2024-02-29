using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    private PlayerInputActions actions;

    public void SetUp()
    {
        actions = new PlayerInputActions();
        actions.Navigation.Enable();
        actions.Navigation.Pause.Enable();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (actions.Navigation.Pause.WasPerformedThisFrame())
        {
            Hide();
        }
    }
}
