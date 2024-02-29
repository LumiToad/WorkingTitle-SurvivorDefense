using System;
using UnityEngine;

public abstract class LevelSection : MonoBehaviour
{
    public event Action Completed;

    public void SetUp()
    {
        foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Stop();
            particleSystem.Clear();
        }
    }

    public virtual void StartSection(int index)
    {
        foreach(var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Play();
        }
    }

    public virtual void LevelSectionUpdate(int distanceToThisSection) { }
    protected void EndSection()
    {
        Completed?.Invoke();

        foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Stop();
            particleSystem.Clear();
        }
    }
}
