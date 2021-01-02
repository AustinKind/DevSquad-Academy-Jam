using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] public int damage = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmgObj = null;
        if ((dmgObj = col.GetComponent<Damageable>()) != null)
        {
            PlayerDamage player = null;
            if ((player = (dmgObj as PlayerDamage)) != null)
                player.SetKnockback(new Vector2(player.transform.position.x - transform.position.x, 1f));
            dmgObj.Hurt(damage);
        }
    }
}
