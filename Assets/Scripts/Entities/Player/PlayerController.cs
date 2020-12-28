using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput input;
    PlayerMovement movement;
    PlayerAnimator animator;

    delegate void OnJumpAction();
    OnJumpAction onJump;

    delegate void OnMovementAction(Vector2 input);
    OnMovementAction onMovement;

    private void Start()
    {
        GetRequiredComponents();
        SetActions();
    }

    private void GetRequiredComponents()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<PlayerAnimator>();
    }

    void SetActions()
    {
        onMovement += movement.Move;
        onJump += movement.Jump;
    }

    private void Update()
    {
        ReadInputs();

        animator.UpdateAnimator(movement.Controller.Rigidbody.velocity, movement.Controller.IsGrounded);
    }

    void ReadInputs()
    {
        movement.UseThroughGround = (input.Movement.y >= -0.02f);
        onMovement.Invoke(input.Movement);

        if (input.Jump)
            onJump.Invoke();
    }
}
