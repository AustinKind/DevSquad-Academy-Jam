using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovement : RigidbodyMovement
{
    public bool UseThroughGround { get; set; }
    [SerializeField] private float jumpVelocity = 8f;

    PlayerMovementType[] movements;
    delegate void CurrentMovementAction(Vector2 input, ref Vector2 moveDirection, bool grounded);
    delegate void CurrentFixedMovementAction(ref Vector2 moveDirection, bool grounded);
    delegate void CurrentJumpAction(ref Vector2 moveDirection);
    CurrentFixedMovementAction currentFixedMovement = null;
    CurrentMovementAction currentMovement = null;
    CurrentJumpAction currentJump = null;

    protected override void Start()
    {
        base.Start();
        movements = GetComponentsInChildren<PlayerMovementType>();
        foreach (PlayerMovementType moveType in movements)
            moveType.Initialize(this);
    }

    private void Update()
    {
        currentJump = null;
        currentMovement = null;
        currentFixedMovement = null;
        foreach (PlayerMovementType moveType in movements)
        {
            if(moveType.ShouldUseMovement())
            {
                currentMovement = moveType.Movement;
                currentFixedMovement = moveType.FixedMovement;
                currentJump = moveType.Jump;
            }
        }
    }

    public void Jump()
    {
        //Do nothing if the player is not grounded
        if (!controller.IsGrounded) return;

        if (currentJump == null)
            DefaultJump();
        else
            currentJump.Invoke(ref moveDirection);
    }

    public void Move(Vector2 input)
    {
        if (currentMovement == null)
            DefaultMovement(input);
        else
            currentMovement.Invoke(input, ref moveDirection, controller.IsGrounded);
    }

    protected override void FixedUpdate()
    {
        if (currentFixedMovement == null)
            DefaultFixedMovement();
        else
            currentFixedMovement.Invoke(ref moveDirection, controller.IsGrounded);

        moveDirection = controller.Move(moveDirection, UseThroughGround);
    }

    void DefaultMovement(Vector2 input)
    {
        moveDirection.x = input.x * moveSpeed;
    }

    void DefaultFixedMovement()
    {
        if (!controller.IsGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = Mathf.Clamp(moveDirection.y, 0, float.MaxValue);
    }
    void DefaultJump()
    {
        moveDirection.y = jumpVelocity;
    }
}
