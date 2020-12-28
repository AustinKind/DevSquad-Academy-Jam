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

    public void UpdateAnimator(Vector2 vel, bool grounded)
    {
        ani.SetFloat("Movement", vel.magnitude);
        ani.SetFloat("velocityY", (grounded) ? 0 : vel.y);
        ani.SetBool("grounded", grounded);

        if(Mathf.Abs(vel.x) > 0.02f)
            rend.flipX = (vel.x < 0);
    }
}
