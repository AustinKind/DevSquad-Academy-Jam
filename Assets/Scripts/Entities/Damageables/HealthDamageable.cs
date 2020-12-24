using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageable : Damageable
{
    [SerializeField] private int health;
    public override void Hurt(int dmg)
    {
        health = Mathf.Clamp(health - dmg, 0, int.MaxValue);
        if (health <= 0)
            Destroy(gameObject);
    }
}
