using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovement : RigidbodyMovement
{
    public bool UseThroughGround { get; set; }
    [SerializeField] private float jumpVelocity = 8f;

    PlayerMovementType[] movements;
    delegate void CurrentFixedMovementAction(ref Vector2 moveDirection, bool grounded);
    delegate void CurrentJumpAction(ref Vector2 moveDirection, bool grounded);
    CurrentFixedMovementAction currentFixedMovement = null;
    CurrentJumpAction currentJump = null;

    public bool freezeMovement = false;
    Vector2 input;
    private AudioController audioController;

    protected override void GetRequiredComponents()
    {
        base.GetRequiredComponents();
        audioController = AudioController.Instance;
        movements = GetComponentsInChildren<PlayerMovementType>();
        foreach (PlayerMovementType moveType in movements)
            moveType.Initialize(this);
    }

    private void Update()
    {
        currentJump = null;
        currentFixedMovement = null;
        foreach (PlayerMovementType moveType in movements)
        {
            if(moveType.ShouldUseMovement())
            {
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

    public void UpdateInput(Vector2 i)
    {
        input = i;

        if (freezeMovement)
            input = Vector2.zero;

        foreach (PlayerMovementType moveType in movements)
            moveType.SetInput = i;
    }

    protected override void FixedUpdate()
    {
        if (currentFixedMovement == null)
            DefaultFixedMovement();
        else
            currentFixedMovement.Invoke(ref moveDirection, controller.IsGrounded);

        moveDirection = controller.Move(moveDirection, UseThroughGround);
    }

    void DefaultFixedMovement()
    {
        float x = input.x * moveSpeed;
        if (!controller.IsGrounded)
        {
            if (Mathf.Abs(input.x) > 0.02f)
                moveDirection.x = x;
            moveDirection.y -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            moveDirection.x = x;
            moveDirection.y = Mathf.Clamp(moveDirection.y, 0, float.MaxValue);
        }
    }

    void DefaultJump()
    {
        if (!controller.IsGrounded) return;
        audioController.PlaySound("jump");
        moveDirection.y = jumpVelocity;
    }

    public void RemoveVelocity()
    {
        moveDirection = Vector2.zero;
        Controller.Rigidbody.velocity = Vector2.zero;
    }
}
