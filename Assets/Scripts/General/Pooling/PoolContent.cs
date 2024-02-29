using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolContent 
{
    [SerializeField, InfoBox("Object does not integrate Ipoolable", InfoMessageType.Error, "LegalToPool")]
    private GameObject objectToPool;
    [SerializeField]
    private int amount = 50;
    [SerializeField]
    private bool extendable = true;
    [field:SerializeField, HideInEditorMode]
    public int returnedCount { get; private set; } = 0;

    public List<GameObject> outOfPool { get; private set; } = new List<GameObject>();

    private List<GameObject> inPool = new List<GameObject>();
    private GameObject poolHolder;
    private bool locked = false;

    public void SetUp(Transform parent)
    {
        poolHolder = new GameObject(objectToPool.name + " Pool");
        poolHolder.transform.SetParent(parent);

        IncreasePool(amount);
    }

    public bool IsPooling<T>(string name = "")
    {
        if(name == "")
        {
            return objectToPool.GetComponent<T>() != null;
        }

        return objectToPool.GetComponent<T>() != null && objectToPool.name == name;
    }

    public T Get<T>(string name = "")
    {
        if(locked || !IsPooling<T>(name))
        {
            return default(T);
        }

        if(inPool.Count == 0)
        {
            if (!extendable)
            {
                return default(T);
            }

            IncreasePool(1);
        }

        var value = inPool[0];

        inPool.Remove(value);
        outOfPool.Add(value);

        EnableGameObject(value);
        return value.GetComponent<T>();
    }

    public bool IsPooling(string name) => objectToPool.name == name;

    public void Lock(bool state) => locked = state;

    public GameObject Get(string name)
    {
        if (locked | !IsPooling(name))
        {
            return null;
        }

        if (inPool.Count == 0)
        {
            if (!extendable)
            {
                return null;
            }

            IncreasePool(1);
        }

        var value = inPool[0];

        inPool.Remove(value);
        outOfPool.Add(value);

        EnableGameObject(value);

        return value;
    }

    public void Return<T>(GameObject objectToReturn)
    {
        if (!IsPooling<T>())
        {
            return;
        }

        InternalReturn(objectToReturn);
    }

    public List<T> GetActive<T>()
    {
        if (!IsPooling<T>())
        {
            return new List<T>();
        }

        var value = new List<T>();

        foreach(var gameObject in outOfPool)
        {
            value.Add(gameObject.GetComponent<T>());
        }

        return value;
    }

    public void ReturnAll()
    {
        var toReturn = new List<GameObject>();

        foreach(var item in outOfPool)
        {
            toReturn.Add(item.gameObject);
        }

        foreach(var ob in toReturn)
        {
            InternalReturn(ob);
        }
    }

    private void IncreasePool(int poolAmount)
    {
        for (int i = 0; i < poolAmount; i++)
        {
            var spawn = GameObject.Instantiate(objectToPool);
            DisableGameObject(spawn);
            inPool.Add(spawn);
        }
    }

    private void InternalReturn(GameObject objectToReturn)
    {
        outOfPool.Remove(objectToReturn);
        inPool.Add(objectToReturn);

        if (inPool.Count + outOfPool.Count > amount)
        {
            amount = inPool.Count + outOfPool.Count;
        }

        returnedCount++;

        DisableGameObject(objectToReturn);
    }

    private void DisableGameObject(GameObject toDisable)
    {
        toDisable.transform.SetParent(poolHolder.transform);
        toDisable.gameObject.SetActive(false);
        toDisable.transform.position = poolHolder.transform.position;

        toDisable.GetComponent<IPoolable>().OnReturnedToPool();
    }

    private void EnableGameObject(GameObject toEnable)
    {
        toEnable.transform.SetParent(null);
        toEnable.gameObject.SetActive(true);

        toEnable.GetComponent<IPoolable>().OnTakenFromPool();
    }


   

    //Used by Odin
    private bool LegalToPool()
    {
        if (objectToPool != null && objectToPool.GetComponent<IPoolable>() == null) return true;
        return false;
    }

   
}
