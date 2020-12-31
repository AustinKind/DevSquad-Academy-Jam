using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolParent : MonoBehaviour
{
    ObjectPool pool;

    public void Initialize(ObjectPool p)
    {
        pool = p;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
