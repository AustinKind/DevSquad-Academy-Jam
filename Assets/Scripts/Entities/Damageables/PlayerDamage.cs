using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Damageable
{
    public override void Hurt(int dmg)
    {
        PlayerStatusManager.Instance.ModifyHealth(-dmg);
    }
}
