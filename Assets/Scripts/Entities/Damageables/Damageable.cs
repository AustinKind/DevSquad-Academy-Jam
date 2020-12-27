using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public delegate void OnHurtAction();
    public OnHurtAction onHurt;

    public virtual void Start()
    {
        onHurt = OnHurt;
    }

    public virtual void Hurt(int dmg)
    {
        onHurt.Invoke();
    }

    protected virtual void OnHurt() { }
}
