using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public virtual void Start()
    {

    }

    public abstract void Shoot(Vector2 dir);
    public abstract void Reload();
}
