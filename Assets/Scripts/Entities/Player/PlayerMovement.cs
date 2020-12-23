using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RigidbodyMovement
{
    public void Jump()
    {
        //Do nothing if the player is not grounded
        if (!controller.IsGrounded) return;

    }

    public void Move(Vector2 input)
    {
        moveDirection.x = input.x * moveSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
