using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Damageable
{
    public override void Start()
    {
        base.Start();
    }

    public override void Hurt(int dmg)
    {
        base.Hurt(dmg);
        PlayerStatusManager.Instance.ModifyHealth(-dmg);
    }
}
