using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Attach this to an object to make it destructable. You can specify the number of hits it can take before getting destroyed and the frames of its damaged state.
public class Destructible : HealthDamageable
{

    // All of the different frames of the object slowly breaking down.
    [SerializeField] private Sprite[] destructableFrames;

    private float percentHealth;
    private float threshhold;

    SpriteRenderer spriteRenderer;
    
    // Any damage received is converted to 1 damage.
    public override void Hurt(int dmg)
    {
        // base.Hurt(dmg);
        Debug.Log("Hit!");
        base.Hurt(dmg);
        UpdateSprite();
    }

    protected override void OnHurt()
    {
        // TODO
    }

    // Picks the appropriate frame depending on the percentage of health left.
    private void UpdateSprite()
    {
        percentHealth = health / (float)maxHealth;

        int index = Mathf.Clamp(destructableFrames.Length - Mathf.RoundToInt(percentHealth / threshhold), 0, destructableFrames.Length - 1);
        spriteRenderer.sprite = destructableFrames[index];
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = destructableFrames[0];
        threshhold = 1f / destructableFrames.Length;
    }
}