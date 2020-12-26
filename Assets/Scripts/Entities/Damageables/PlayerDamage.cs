using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Damageable
{
    public delegate void OnHurtAction();
    public OnHurtAction onHurt;

    public override void Hurt(int dmg)
    {
        PlayerStatusManager.Instance.ModifyHealth(-dmg);
        onHurt.Invoke();
    }
}
