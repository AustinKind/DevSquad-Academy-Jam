using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Attach this to an object to make it destructable. You can specify the number of hits it can take before getting destroyed and the frames of its damaged state.
public class Destructible : Damageable
{
    public SpriteRenderer spriteRenderer;

    // All of the different frames of the object slowly breaking down.
    public Sprite[] destructableFrames;

    // Number of hits to destroy the object
    [SerializeField] private int hitsToDestroy = 5;

    private int health;
    private float percentHealth;
    private float threshhold;

    // Any damage received is converted to 1 damage.
    public override void Hurt(int dmg)
    {
        // base.Hurt(dmg);
        Debug.Log("Hit!");
        health = Mathf.Clamp(health - 1, 0, int.MaxValue);
        if (health <= 0)
            Destroy(gameObject);
        UpdateSprite();
    }

    protected override void OnHurt()
    {
        // TODO
    }

    // Picks the appropriate frame depending on the percentage of health left.
    private void UpdateSprite()
    {
        percentHealth = (float)health / hitsToDestroy * 100;
        for (int i = 0; i < destructableFrames.Length; i++)
        {
            if (percentHealth > 100 - ((i+1) * threshhold) && percentHealth <= 100 - (threshhold * i))
            {
                spriteRenderer.sprite = destructableFrames[i];
                break;
            }        
        }
    }

    public override void Start ()
    {
        spriteRenderer.sprite = destructableFrames[0];
        health = hitsToDestroy;
        threshhold = 1f / destructableFrames.Length * 100;
    }
}
