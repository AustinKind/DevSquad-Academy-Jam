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
    delegate void CurrentJumpAction(ref Vector2 moveDirection, bool grounded);
    CurrentFixedMovementAction currentFixedMovement = null;
    CurrentMovementAction currentMovement = null;
    CurrentJumpAction currentJump = null;

    public bool freezeMovement = false;

    protected override void GetRequiredComponents()
    {
        base.GetRequiredComponents();

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

    public void FreezeMovement()
    {
        StartCoroutine(freezingMovement());
        IEnumerator freezingMovement()
        {
            freezeMovement = true;
            while(moveDirection.y > float.Epsilon && !controller.IsGrounded)
                yield return null;
            freezeMovement = false;
        }
    }

    public void FreezeMovement(float seconds)
    {
        StartCoroutine(freezingMovement());
        IEnumerator freezingMovement()
        {
            freezeMovement = true;
            yield return new WaitForSeconds(seconds);
            freezeMovement = false;
        }
    }

    public void Jump()
    {
        if (currentJump == null)
            DefaultJump();
        else
            currentJump.Invoke(ref moveDirection, controller.IsGrounded);
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
        if (freezeMovement) return;
        float x = input.x * moveSpeed;
        if (controller.IsGrounded)
            moveDirection.x = x;
        else if (Mathf.Abs(input.x) > 0.02f)
            moveDirection.x = x;
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
        if (!controller.IsGrounded) return;
        moveDirection.y = jumpVelocity;
    }
}
