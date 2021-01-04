using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : HealthDamageable
{
    public Animator anim;
    public Sprite deathSprite;
    public BoxCollider2D boxCollider;

    public override void OnHealthGone()
    {
        StartCoroutine("TheEnd");
    }

    IEnumerator TheEnd()
    {
        anim.enabled = false;
        render.sprite = deathSprite;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(3);
        GameSceneController.Instance.NextLevel();
    }
}
