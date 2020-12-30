using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Damageable
{
    private SpriteRenderer renderer;
    [SerializeField] private float invulnerabilityTime = 2f;
    [SerializeField] private float intervalTime = 0.25f;
    [SerializeField] private Color flashingColor = new Color(1f, 0.1f, 0.1f, 0.39f);
    private bool isInvulnerable = false;
    private bool looping = true;

    public override void Start()
    {
        base.Start();
        renderer = transform.Find("CharacterSprite").gameObject.GetComponent<SpriteRenderer>();
    }

    public override void Hurt(int dmg)
    {
        if (!isInvulnerable)
        {
            base.Hurt(dmg);
            PlayerStatusManager.Instance.ModifyHealth(-dmg);
            Knockback();
            StartCoroutine("Invulnerable");
        }
    }

    public void Knockback()
    {
        Debug.Log("Apply knockback!");
    }

    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        StartCoroutine("Waiting");
        while (looping)
        {
            renderer.color = flashingColor;
            yield return new WaitForSeconds(intervalTime);
            renderer.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(intervalTime);
        }
        renderer.color = new Color(255, 255, 255, 1);
        isInvulnerable = false;
        looping = true;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        looping = false;
    }
}
