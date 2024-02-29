using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementCard : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI title;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI description;
    [SerializeField, FoldoutGroup("References")]
    private Image fill;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI percent;

    public void SetUp(AbstractAchivement achivement)
    {
        this.title.text = achivement.GetName();
        this.description.text = achivement.GetDescription();

        percent.text = achivement.GetProgressString();
        fill.fillAmount = achivement.GetProgressPercent();
    }
}
