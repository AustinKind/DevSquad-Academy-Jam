using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public GameObject pooledObject;
    List<PooledObject> pool;

    Transform poolParent;

    public ObjectPool()
    {
        pooledObject = null;
        pool = new List<PooledObject>();
        CreatePoolParent();
    }

    public ObjectPool(GameObject obj)
    {
        pooledObject = obj;
        pool = new List<PooledObject>();
        CreatePoolParent();
    }

    void CreatePoolParent()
    {
        poolParent = new GameObject().transform;
        poolParent.name = $"_{pooledObject.name}-Pool";
    }

    public PooledObject GrabFromPool
    {
        get
        {
            PooledObject obj = GrabUsableObject;
            if (obj == null)
            {
                obj = new PooledObject(GameObject.Instantiate(pooledObject, poolParent));
                obj.currentlyInPool = false;
            }
            return obj;
        }
    }

    public PooledObject GrabUsableObject
    {
        get
        {
            for(int i = 0; i < pool.Count; i++)
            {
                if (pool[i].currentlyInPool)
                {
                    pool[i].currentlyInPool = false;
                    return pool[i];
                }
            }
            return null;
        }
    }
}

public class PooledObject
{
    public GameObject obj;
    public bool currentlyInPool = true;

    public PooledObject(GameObject o)
    {
        currentlyInPool = true;
        obj = o;
    }
}