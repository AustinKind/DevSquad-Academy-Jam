using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackMovement : PlayerMovementType
{
    [SerializeField] private float knockbackForce = 4f;
    private Vector2 knockback;
    private float knockbackTimer = 0f;

    public void KnockPlayer(Vector2 dir, float time)
    {
        knockbackTimer = time;
        knockback = dir * knockbackForce;
    }

    public override void FixedMovement(ref Vector2 moveDirection, bool grounded)
    {
        if (!grounded)
            knockback.y -= playerMovement.Gravity * Time.fixedDeltaTime;
        else if (knockback.y <= 0)
        {
            //Lower knockback time a second time (to end it quicker)
            knockbackTimer -= Time.fixedDeltaTime;
            knockback = Vector2.zero;
        }

        moveDirection = knockback;

        if (knockbackTimer > 0)
            knockbackTimer -= Time.fixedDeltaTime;
    }

    public override void Jump(ref Vector2 moveDirection, bool grounded)
    {
    }

    public override bool ShouldUseMovement()
    {
        return (knockbackTimer > 0);
    }
}
