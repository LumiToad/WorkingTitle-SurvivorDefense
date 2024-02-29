using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    private const float exponentialXPBoost = 0.1f;

    [InfoBox("Error: Preview Fill Speed must be higher than regular Fill speed",InfoMessageType = InfoMessageType.Error, VisibleIf = "ShowErrorInfoBox")]

    [SerializeField, SuffixLabel("fills / second",overlay: true)]
    private float fillSpeed;
    [SerializeField, SuffixLabel("fills / second", overlay: true)]
    private float previewFillSpeed;

    [FoldoutGroup("References"),SerializeField]
    private Slider slider;
    [FoldoutGroup("References"), SerializeField]
    private Animator fillAnimator;
    [FoldoutGroup("References"),SerializeField]
    private Image preview;

    private List<float> toFill = new List<float>();

    public void UpdateValue(float current, float max)
    {
        float percent = Mathf.Clamp(current / max, 0, 1);
        toFill.Add(percent);
    }

    private void Start()
    {
        StartCoroutine(Fill());
    }

    private void Update()
    {
        var amount = GetHighestQueuedPercent();

        if (Mathf.Abs(amount - preview.fillAmount) < 0.001f) return;

        if (preview.fillAmount < amount && preview.fillAmount < 1)
        {
            preview.fillAmount += previewFillSpeed * Time.deltaTime * GetFillBonus();
        }
        
        if (preview.fillAmount > amount)
        {
            preview.fillAmount -= previewFillSpeed * 2 * Time.deltaTime * GetFillBonus();
        }
    }

    private IEnumerator Fill()
    {
        while (true)
        {
            while (toFill.Count > 0)
            {
                fillAnimator.SetBool("filling", true);

                var percent = toFill[0];

                if(percent < slider.value)
                {
                    slider.value = 0;
                }

                while (slider.value < percent && slider.value < 1)
                {
                    slider.value += Time.deltaTime * fillSpeed * GetFillBonus();
                    yield return new WaitForEndOfFrame();
                }

                if(toFill.Count == 1)
                {
                    preview.fillAmount = 0;
                }

                toFill.RemoveAt(0);
                fillAnimator.SetBool("filling", false);
            }

            yield return null;
        }
    }

    private float GetHighestQueuedPercent()
    {
        float highest = 0;
        foreach(var percent in toFill)
        {
            if (percent > highest)
            {
                highest = percent;
            }
        }
        return highest;
    }

    private bool ShowErrorInfoBox()
    {
        return fillSpeed > previewFillSpeed;
    }

    private float GetFillBonus()
    {
        return (1 + exponentialXPBoost * toFill.Count);
    }
}
