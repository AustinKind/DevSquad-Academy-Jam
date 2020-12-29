using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot(Vector2 dir)
    {
        if (!canShootGun) return;

        //SHOOT

        StartCoroutine(RegisterShot());
    }

    public override void Reload()
    {

    }

}
