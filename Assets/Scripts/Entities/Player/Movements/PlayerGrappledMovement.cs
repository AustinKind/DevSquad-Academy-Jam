using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappledMovement : PlayerMovementType
{
    [SerializeField] private float swingSpeed = 4f;
    [SerializeField] private float jumpVelocity = 8f;

    GrappleHook grapple;

    private void Start()
    {
        grapple = GetComponentInChildren<GrappleHook>();
    }

    public override void Jump(ref Vector2 moveDirection, bool grounded)
    {
        moveDirection.y = jumpVelocity;
        grapple.Unhook();
    }

    public override void Movement(Vector2 input, ref Vector2 moveDirection, bool grounded)
    {
        //TODO

        //PLACEHOLDER MOVEMENT
        moveDirection = (grapple.HookTransform.position - (transform.position + (Vector3)grapple.HookOffset)) * swingSpeed;
    }

    public override bool ShouldUseMovement()
    {
        if (grapple == null) return false;

        bool grappled = grapple.IsHooked;
        return grappled;
    }

    public override void FixedMovement(ref Vector2 moveDirection, bool grounded)
    {
    }

}
