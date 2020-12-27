using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageable : Damageable
{
    [SerializeField] protected int health = 10;
    protected int maxHealth;

    public delegate void HealthZeroedOut();
    public HealthZeroedOut onHealthZeroed;

    public override void Start()
    {
        base.Start();
        maxHealth = health;
        onHealthZeroed = OnHealthGone;
    }

    public override void Hurt(int dmg)
    {
        if(dmg > 0)
            base.Hurt(dmg);
        
        health = Mathf.Clamp(health - dmg, 0, maxHealth);
        if (health <= 0)
            onHealthZeroed.Invoke();
    }

    public virtual void OnHealthGone()
    {
        Destroy(gameObject);
    }
}
