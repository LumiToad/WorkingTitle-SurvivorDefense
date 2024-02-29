using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlockCanvas : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Image icon;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(AbstractWeapon recentUnlock, bool error = false)
    {
        anim.Play(error ? "LevelUpCanvasError" : "LevelUpCanvas");
        icon.sprite = recentUnlock.sprite;
    }
}
