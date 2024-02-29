using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ArenaTimeDisplay : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI text;

    public void ShowTime(float timer)
    {
        var minutes = 0;
        while(timer >= 60)
        {
            timer -= 60;
            minutes++;
        }
        var seconds = Mathf.RoundToInt(timer);

        string minText = minutes.ToString();
        while(minText.Length < 2)
        {
            minText = "0" + minText;
        }

        string secText = seconds.ToString();
        while(secText.Length < 2)
        {
            secText = "0" + secText;
        }

        text.text = $"{minText}:{secText}";
    }
}
