using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectRef : MonoBehaviour
{
    int index = -1;
    ObjectPool pool;

    public void Initialize(ObjectPool p, int i)
    {
        index = i;
        pool = p;
    }

    public void PlaceBackInPool()
    {
        pool.ReturnToPool(index);
    }
}
