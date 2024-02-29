using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : DamageZone
{
    private Animator anim;
    private ParticleSystem VFX;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        VFX = GetComponentInChildren<ParticleSystem>();
    }

    public void SetUp(int damage, IDamageSource source)
    {
        base.damage = damage;
        base.overrideSource = source;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        anim.Play("Show");
    }

    public void Hide()
    {
        anim.Play("Hide");
    }

    public void StartVFX() => VFX?.Play();
    public void StopVFX() => VFX?.Stop();
}
