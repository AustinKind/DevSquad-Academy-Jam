using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Damageable
{
    private SpriteRenderer render;
    [SerializeField] private float invulnerabilityTime = 2f;
    [SerializeField, Range(0f, 1f)] private float knockbackPercent = 0.5f;
    [SerializeField] private float intervalTime = 0.25f;
    [SerializeField] private Color flashingColor = new Color(1f, 0.1f, 0.1f, 0.39f);
    private bool isInvulnerable = false;
    private bool looping = true;
    private AudioController audioController;

    Vector2 tempKnockback = Vector2.zero;

    public override void Start()
    {
        base.Start();
        render = transform.Find("CharacterSprite").gameObject.GetComponent<SpriteRenderer>();
        audioController = AudioController.Instance;
    }

    public override void Hurt(int dmg)
    {
        if (!isInvulnerable)
        {
            audioController.PlaySound("hurt");
            base.Hurt(dmg);
            PlayerStatusManager.Instance.ModifyHealth(-dmg);
            PlayerStatusManager.Instance.ApplyKnockback(tempKnockback, invulnerabilityTime * knockbackPercent);
            if (PlayerStatusManager.Instance.IsAlive)
                StartCoroutine("Invulnerable");
        }
    }

    public void SetKnockback(Vector2 dir)
    {
        tempKnockback = dir;
    }

    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        StartCoroutine("Waiting");
        while (looping)
        {
            render.color = flashingColor;
            yield return new WaitForSeconds(intervalTime);
            render.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(intervalTime);
        }
        render.color = new Color(255, 255, 255, 1);
        isInvulnerable = false;
        looping = true;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        looping = false;
    }
}
