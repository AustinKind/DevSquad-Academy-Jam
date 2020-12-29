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

    public void ReturnToPool(int index)
    {
        PooledObject pObj = pool[index];
        pObj.obj.SetActive(false);
        pObj.currentlyInPool = true;
    }

    public PooledObject GrabFromPool
    {
        get
        {
            PooledObject obj = GrabUsableObject;
            if (obj == null)
            {
                obj = new PooledObject(GameObject.Instantiate(pooledObject, poolParent), pool.Count);

                PooledObjectRef pRef = obj.obj.AddComponent<PooledObjectRef>();
                pRef.Initialize(this, pool.Count);
                obj.currentlyInPool = false;

                pool.Add(obj);
            }
            return obj;
        }
    }

    PooledObject GrabUsableObject
    {
        get
        {
            for(int i = 0; i < pool.Count; i++)
            {
                if (pool[i].currentlyInPool)
                {
                    pool[i].obj.SetActive(true);
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

    int index;
    public int Index => index;

    public PooledObject(GameObject o, int i)
    {
        currentlyInPool = true;
        index = i;
        obj = o;
    }
}