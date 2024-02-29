using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class IntStat
{
    public int totalValue { get { return Mathf.RoundToInt(value * (1 + percentBonus)) + flatBonus; } set { } }

    [Title("Total Value"), ShowInInspector, HideInEditorMode, DisableInPlayMode, PropertyOrder(-1)]
    private int total { get { return totalValue; } }

    [Title("Settings")]
    [SerializeField, DisableInPlayMode]
    public int value;
    [SerializeField, HideInPlayMode, OnValueChanged("SetDefaultCaps")]
    private bool capped;

    [Title("Caps")]
    [SerializeField, DisableInPlayMode, ShowIf("capped")]
    private int maxValue;
    [SerializeField, DisableInPlayMode, ShowIf("capped")]
    private int minValue;

    [Title("Modifications")]
    [HideInEditorMode, DisableInPlayMode]
    public float percentBonus;
    [HideInEditorMode, DisableInPlayMode]
    public int flatBonus;

    public IntStat(int value)
    {
        this.value = value;
    }

    public IntStat(IntStat other)
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

    public void SetTo(int value)
    {
        flatBonus -= totalValue - value;
    }

    public static implicit operator int(IntStat stat)
    {
        ClampTotalValue(stat);
        return stat.totalValue;
    }

    public static IntStat operator +(IntStat a, int b)
    {
        a.flatBonus += b;
        ClampTotalValue(a);
        return a;
    }

    public static IntStat operator -(IntStat a, int b)
    {
        a.flatBonus -= b;
        ClampTotalValue(a);
        return a;
    }

    public static IntStat operator *(IntStat a, int b)
    {
        a.percentBonus += b;
        ClampTotalValue(a);
        return a;
    }

    public static IntStat operator /(IntStat a, int b)
    {
        a.percentBonus -= b;
        ClampTotalValue(a);
        return a;
    }

    private static void ClampTotalValue(IntStat a)
    {
        if (a.capped)
        {
            if(a.total > a.maxValue)
            {
                var delta = a.total - a.maxValue;
                a.flatBonus -= delta;
            }
            if(a.total < a.minValue)
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
