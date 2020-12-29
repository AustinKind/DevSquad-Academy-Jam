﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PooledObjectRef poolRef;

    void Start()
    {
        poolRef = GetComponent<PooledObjectRef>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmgObj = null;
        if ((dmgObj = collision.GetComponent<Damageable>()) != null)
        {
            dmgObj.Hurt(5);
        }

        if (poolRef) poolRef.PlaceBackInPool();
    }
}
