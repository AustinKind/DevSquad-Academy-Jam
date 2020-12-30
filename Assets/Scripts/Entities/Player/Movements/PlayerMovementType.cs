using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementType : MonoBehaviour
{
    protected PlayerMovement playerMovement;
    protected Vector2 input;
    public Vector2 SetInput
    {
        set { input = value; }
    }

    public void Initialize(PlayerMovement movement)
    {
        playerMovement = movement;
    }

    public abstract bool ShouldUseMovement();
    public abstract void FixedMovement(ref Vector2 moveDirection, bool grounded);
    public abstract void Jump(ref Vector2 moveDirection, bool grounded);
}
