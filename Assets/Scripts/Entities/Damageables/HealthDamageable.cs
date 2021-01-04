using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageable : Damageable
{
    [SerializeField] protected int health = 10;
    protected int maxHealth;
    public SpriteRenderer render;
    [SerializeField] private Color flashingColor = new Color(1f, 0.1f, 0.1f, 0.39f);

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
        StartCoroutine("Flash");
        health = Mathf.Clamp(health - dmg, 0, maxHealth);
        if (health <= 0)
            onHealthZeroed.Invoke();
    }

    IEnumerator Flash()
    {
        render.color = flashingColor;
        yield return new WaitForSeconds(0.25f);
        render.color = new Color(255, 255, 255, 1);
    }

    public virtual void OnHealthGone()
    {
        Destroy(gameObject);
    }
}
