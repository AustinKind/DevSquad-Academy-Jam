using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator ani;
    SpriteRenderer rend;

    private void Start()
    {
        GetRequiredComponents();
    }

    void GetRequiredComponents()
    {
        ani = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void UpdateAnimator(Vector2 vel, Vector2 shooting, bool grounded)
    {
        int dir = 1;
        int aimY = 0;
        if(Mathf.Abs(shooting.y) > 0.02f)
            aimY = (shooting.y > 0) ? 1 : -1;

        if (Mathf.Abs(shooting.x) > 0.02f)
        {
            bool backwards = (shooting.x < 0);
            rend.flipX = backwards;
            if ((backwards && vel.x > 0.02f) || (!backwards && vel.x < -0.02f))
                dir = -1;
        }
        else if (Mathf.Abs(vel.x) > 0.02f)
            rend.flipX = (vel.x < 0);

        ani.SetFloat("Movement", vel.magnitude * dir);
        ani.SetFloat("velocityY", (grounded) ? 0 : vel.y);
        ani.SetBool("grounded", grounded);
        ani.SetInteger("aimY", aimY);
    }
}
