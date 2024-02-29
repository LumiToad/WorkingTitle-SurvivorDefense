using Sirenix.OdinInspector;
using System;
using UnityEngine;

[System.Serializable]
public class FloatStat
{
    private float totalValue { get { return (value * (1 + percentBonus) + flatBonus); } set { } }

    [Title("Total Value"), ShowInInspector, HideInEditorMode, DisableInPlayMode, PropertyOrder(-1)]
    public float total { get { return totalValue; } }

    [Title("Settings")]
    [SerializeField, DisableInPlayMode]
    public float value;
    [SerializeField, HideInPlayMode, OnValueChanged("SetDefaultCaps")]
    private bool capped;

    [Title("Caps")]
    [SerializeField, DisableInPlayMode, ShowIf("capped")]
    private float maxValue;
    [SerializeField, DisableInPlayMode, ShowIf("capped")]
    private float minValue;

    [Title("Modifications")]
    [HideInEditorMode, DisableInPlayMode]
    public float percentBonus;
    [HideInEditorMode, DisableInPlayMode]
    public float flatBonus;

    public FloatStat(float value)
    {
        this.value = value;
    }

    public FloatStat(FloatStat other)
    {
        value = other.value;
        percentBonus = other.percentBonus;
        flatBonus = other.flatBonus;
    }

    public void Reset()
    {
        percentBonus = 0;
        flatBonus = 0;
    }

    public void SetTo(float value)
    {
        flatBonus -= totalValue - value;
    }

    public static implicit operator float(FloatStat stat)
    {
        ClampTotalValue(stat);
        return stat.totalValue;
    }

    public static FloatStat operator +(FloatStat a, float b)
    {
        a.flatBonus += b;
        ClampTotalValue(a);
        return a;
    }

    public static FloatStat operator -(FloatStat a, float b)
    {
        a.flatBonus -= b;
        ClampTotalValue(a);
        return a;
    }

    public static FloatStat operator *(FloatStat a, float b)
    {
        a.percentBonus += b;
        ClampTotalValue(a);
        return a;
    }

    public static FloatStat operator /(FloatStat a, float b)
    {
        a.percentBonus -= b;
        ClampTotalValue(a);
        return a;
    }

    private static void ClampTotalValue(FloatStat a)
    {
        if (a.capped)
        {
            if (a.total > a.maxValue)
            {
                var delta = a.total - a.maxValue;
                a.flatBonus -= delta;
            }
            if (a.total < a.minValue)
            {
                var delta = a.minValue - a.total;
                a.flatBonus += delta;
            }
        }
    }

    //Used by Odin
    private void SetDefaultCaps()
    {
        if (capped)
        {
            minValue = 0;
            maxValue = value;
        }
    }
}
