using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //up, down, right, left
    public Gun[] weaponCache;

    private void OnValidate()
    {
        if (weaponCache.Length > 4)
        {
            Debug.LogWarning("Do not go over 4 weapons!");
            Array.Resize(ref weaponCache, 4);
        }
    }
}
