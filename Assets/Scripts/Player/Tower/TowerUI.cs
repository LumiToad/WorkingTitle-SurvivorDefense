using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Image icon;

    public void Show(AbstractWeapon weapon)
    {
        icon.sprite = weapon.sprite;
        GetComponent<Animator>().Play("Show");
    }
}
