using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { semi, auto }
public abstract class Gun : MonoBehaviour
{
    public GunType gunShootType;
    [SerializeField] protected float firerate = 1f;
    protected bool canShootGun;

    public virtual void Start()
    {
        canShootGun = true;
    }

    public abstract void Shoot(Vector2 dir);
    public abstract void Reload();

    protected IEnumerator RegisterShot()
    {
        canShootGun = false;
        yield return new WaitForSeconds(firerate);
        canShootGun = true;
    }
}
