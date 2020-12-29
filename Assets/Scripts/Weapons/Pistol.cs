using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public GameObject bullet;
    public float bulletForce = 8f;
    ObjectPool bulletPool;

    public override void Start()
    {
        base.Start();

        bulletPool = new ObjectPool(bullet);
    }

    public override void Shoot(Vector2 dir)
    {
        if (!canShootGun) return;

        //SHOOT
        PooledObject bullet = bulletPool.GrabFromPool;
        Rigidbody2D bulletBody = bullet.obj.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(dir * bulletForce, ForceMode2D.Force);

        StartCoroutine(RegisterShot());
    }

    public override void Reload()
    {

    }

}
