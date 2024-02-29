using SurvivorDTO;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static Vector2 ToVector2(this Vector3 vec) => new Vector2(vec.x, vec.z);
    public static Vector3 ToVector3(this Vector2 vec) => new Vector3(vec.x, 0, vec.y);

    public static bool IsOnScreen(this GameObject go) => IsOnScreen(go, 0);

    public static bool IsOnScreen(this GameObject go, float tolerance)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(go.transform.position);
        if
        (
            (screenPoint.x < -tolerance || screenPoint.x > Screen.currentResolution.width + tolerance) ||
            (screenPoint.y < -tolerance || screenPoint.y > Screen.currentResolution.height + tolerance)
        )
        {
            return false;
        }
        return true;
    }

    public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
    {
        var dictionary = new Dictionary<TValue, TKey>();
        foreach (var entry in source)
        {
            if (!dictionary.ContainsKey(entry.Value))
                dictionary.Add(entry.Value, entry.Key);
        }
        return dictionary;
    }

    public static bool SaveObjectBinary(this object obj, string name, string fileName)
    {
        return SaveFileUtils.SaveObjectBinary(obj, name, fileName);
    }

    public static bool SaveListBinary<T>(this List<T> list, string name, string fileName)
    {
        return SaveFileUtils.SaveObjectListBinary(list, name, fileName);
    }

    public static bool LoadObjectBinary<T>(this T data, string fileName)
    {
        return SaveFileUtils.LoadObjectBinary<T>(ref data, fileName);
    }

    public static bool LoadListBinary<T>(this List<T> list, T sample, string fileName) where T : new()
    {
        return SaveFileUtils.LoadObjectListBinary<T>(sample, ref list, fileName);
    }

    public static bool LoadGameFileBinary(this GameSaveFile gsf)
    {
        return SaveFileUtils.LoadGameFileBinary(ref gsf);
    }
}