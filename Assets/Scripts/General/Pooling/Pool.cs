using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnReturnedToPool();
    void OnTakenFromPool();
}

public class Pool : MonoBehaviour
{
    private static Pool instance;

    public static event Action<GameObject> returned;

    [SerializeField, TabGroup("Pool tabs","Enemies", SdfIconType.EmojiAngry, TextColor = "red")]
    private List<PoolContent> enemies;
    [SerializeField, TabGroup("Pool tabs","Projectiles", SdfIconType.Bullseye, TextColor = "orange")]
    private List<PoolContent> projectiles;
    [SerializeField, TabGroup("Pool tabs", "Sounds", SdfIconType.MusicNote, TextColor = "blue")]
    private new List<PoolContent> audio;
    [SerializeField, TabGroup("Pool tabs", "Other", SdfIconType.Question)]
    private List<PoolContent> other;

    private List<PoolContent> contents
    {
        get
        {
            var result = new List<PoolContent>();
            result.AddRange(enemies);
            result.AddRange(projectiles);
            result.AddRange(audio);
            result.AddRange(other);

            return result;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        foreach(var content in contents)
        {
            content.SetUp(transform);
        }
    }

    public static T Get<T>(string name = "")
    {
        foreach(var content in instance.contents)
        {
            if (content.IsPooling<T>(name))
            {
                return content.Get<T>(name);
            }
        }

        return default(T);
    }

    public static GameObject Get(string name)
    {
        foreach (var content in instance.contents)
        {
            if (content.IsPooling(name))
            {
                return content.Get(name);
            }
        }

        return null;
    }

    public static void Return<T>(GameObject toReturn)
    {
        foreach(var content in instance.contents)
        {
            if (content.outOfPool.Contains(toReturn))
            {
                content.Return<T>(toReturn);
                returned?.Invoke(toReturn);
                return;
            }
        }

        foreach(var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                content.Return<T>(toReturn);
                returned?.Invoke(toReturn);
                return;
            }
        }
    }

    public static List<T> GetOutOfPools<T>()
    {
        var temp = new List<GameObject>();

        foreach (var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                temp.AddRange(content.outOfPool);
            }
        }

        var result = new List<T>();
        foreach(var ob in temp)
        {
            result.Add(ob.GetComponent<T>());
        }
        return result;
    }

    public static int GetOutOfPoolCount<T>()
    {
        var result = 0;

        foreach (var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                result += content.outOfPool.Count;
            }
        }

        return result;
    }

    public static int GetReturnedToPoolCount<T>()
    {
        var result = 0;
        foreach (var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                result += content.returnedCount;
            }
        }
        return result;
    }

    public static void ReturnAll<T>()
    {
        foreach(var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                content.ReturnAll();
            }
        }
    }

    public static void Lock<T>(bool state)
    {
        foreach (var content in instance.contents)
        {
            if (content.IsPooling<T>())
            {
                content.Lock(state);
            }
        }
    }
}
