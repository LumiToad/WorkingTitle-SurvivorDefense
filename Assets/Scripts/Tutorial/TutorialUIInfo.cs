using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIInfo : MonoBehaviour
{
    public event Action confirmed;

    public void Clicked()
    {
        confirmed?.Invoke();
    }
}
