using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : RigidbodyMovement
{
    public enum PlayerStatus { grounded, airborne }
    [SerializeField] private float jumpVelocity = 8f;

    public void Jump()
    {
        //Do nothing if the player is not grounded
        if (!controller.IsGrounded) return;
        moveDirection.y = jumpVelocity;
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
