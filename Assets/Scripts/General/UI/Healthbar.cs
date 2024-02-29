using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]
    private float fillSpeed = 1;
    [SerializeField]
    private Gradient color;

    [SerializeField, FoldoutGroup("References")]
    private Slider fill;
    private Image fillImage;

    private float targetValue = 1;

    private void Awake()
    {
        fillImage = fill.fillRect.GetComponent<Image>();
    }

    public void SetTo(float percent)
    {
        targetValue = percent;
    }

    private void Update()
    {
        if (fill == null) return;

        if(fill.value < targetValue)
        {
            fill.value += fillSpeed * Time.deltaTime;
        }

        if(fill.value > targetValue)
        {
            fill.value -= fillSpeed * Time.deltaTime;
        }

        if(fillImage != null)
        {
            fillImage.color = color.Evaluate(fill.value);
        }
    }
}
