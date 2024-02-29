using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponTooltip : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI title;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI content;


    private void Awake()
    {
        StopAllCoroutines();
    }

    public void Show(AbstractWeapon toShow)
    {
        title.text = toShow.Name;
        content.text = toShow.Description;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
