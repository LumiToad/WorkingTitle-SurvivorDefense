using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager instance;

    [SerializeField, InlineEditor]
    private List<AbstractWeapon> tutorialWeapons = new List<AbstractWeapon>();
    [SerializeField, InlineEditor]
    private List<AbstractWeapon> allWeapons = new List<AbstractWeapon>();


    private static bool tutorialMode = false;
    private static AbstractWeapon lastWeaponTemplate;

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
    }

    public static bool GetRandomWeapon(IWeaponHolder to)
    {
        var weapon = Instantiate(GetRandomWeaponTemplate());
        if (!to.TryAddWeapon(weapon))
        {
            Destroy(weapon);
            return false;
        }

        return true;
    }

    public static bool GetWeapon(string gameObjectName, IWeaponHolder to)
    {
        foreach (var w in instance.allWeapons)
        {
            if (w.gameObject.name == gameObjectName)
            {
                var weapon = Instantiate(w);
                if (!to.TryAddWeapon(weapon))
                {
                    Destroy(weapon);
                    return false;
                }

                lastWeaponTemplate = w;
                return true;
            }
        }

        Debug.LogError($"Error: Weapon {gameObjectName} not registered in weapon manage");

        return false;
    }

    private static AbstractWeapon GetRandomWeaponTemplate()
    {
        AbstractWeapon weapon = null;
        if (tutorialMode)
        {
            weapon = instance.tutorialWeapons.GetRandom();
            while(weapon == lastWeaponTemplate)
            {
                weapon = instance.tutorialWeapons.GetRandom();
            }
        }
        else
        {
            weapon = instance.allWeapons.GetRandom();
            while (weapon == lastWeaponTemplate)
            {
                weapon = instance.allWeapons.GetRandom();
            }
        }

        lastWeaponTemplate = weapon;
        return weapon;
    }

    public static void SetTutorialMode(bool state) => tutorialMode = state;

    public static void TransferWeapon(AbstractWeapon weapon, IWeaponHolder from, IWeaponHolder to)
    {
        from.RemoveWeapon(weapon);

        if (!to.TryAddWeapon(weapon))
        {
            from.TryAddWeapon(weapon);
        }
    }
}
