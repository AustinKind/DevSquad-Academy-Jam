using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public GameObject bullet;
    public Vector2 spawnOffset = Vector2.up;
    public float bulletForce = 8f;
    private AudioController audioController;
    ObjectPool bulletPool;

    public override void Start()
    {
        base.Start();
        audioController = AudioController.Instance;
        if(bullet == null)
        {
            Debug.LogWarning("NO BULLET OBJECT ON PISTOL");
            return;
        }

        bulletPool = new ObjectPool(bullet);
    }

    public override void Shoot(Vector2 dir)
    {
        if (!canShootGun || bulletPool == null) return;

        //SHOOT
        audioController.PlaySound("shoot");
        PooledObject bullet = bulletPool.GrabFromPool;
        Rigidbody2D bulletBody = bullet.obj.GetComponent<Rigidbody2D>();

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.obj.transform.position = transform.position + (Vector3)(spawnOffset + (dir * 0.325f));
        bullet.obj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        bulletBody.AddForce(dir * bulletForce, ForceMode2D.Impulse);

        StartCoroutine(RegisterShot());
    }

    public override void Reload()
    {

    }

}
