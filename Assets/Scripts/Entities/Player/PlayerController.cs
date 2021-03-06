﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput input;
    PlayerMovement movement;

    PlayerBombHandler bombHandler;

    PlayerAnimator animator;
    GunController gunController;


    delegate void OnJumpAction();
    OnJumpAction onJump;

    delegate bool BombHandlerAction();
    BombHandlerAction sendDefuse;

    delegate void OnMovementAction(Vector2 input);
    OnMovementAction onMovement;

    public delegate void OnNextLevelAction();
    public OnNextLevelAction onNextLevel;

    private bool disabled = false;

    private void Start()
    {
        GetRequiredComponents();
        SetActions();
    }

    private void GetRequiredComponents()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();

        bombHandler = GetComponentInChildren<PlayerBombHandler>();

        animator = GetComponentInChildren<PlayerAnimator>();
        gunController = GetComponentInChildren<GunController>();
    }

    void SetActions()
    {
        onMovement = movement.UpdateInput;
        onJump = movement.Jump;

        sendDefuse = bombHandler.SendDefuseToBombs;

        gunController.ActivateTimeScaler(() => { return input.SelectWeapon; });

        onNextLevel = bombHandler.ResetBombs;
        onNextLevel += bombHandler.ResetInputHelper;
    }

    private void Update()
    {
        ReadInputs();
    }

    public void DisableInput(bool _disabled)
    {
        disabled = _disabled;
    }

    void ReadInputs()
    {
        movement.UseThroughGround = (input.Movement.y >= -0.02f);
        if (!disabled)
            onMovement.Invoke(input.Movement);
        else
        {
            onMovement.Invoke(new Vector2(0, 0));
            return;
        }
           
        if (input.Jump)
            onJump.Invoke();
        if (input.Defuse)
        {
            if (sendDefuse.Invoke())
            {
                //Defused all the bombs
                GameSceneController.Instance.NextLevel();
            }
        }

        gunController.OpenWeaponWheel(input.SelectWeapon);

        Vector2 shoot = gunController.ShootInput(input.Shoot);
        animator.UpdateAnimator(movement.Controller.Rigidbody.velocity, shoot, movement.Controller.IsGrounded);
    }

    public void RemoveVelocity()
    {
        movement.RemoveVelocity();
    }
}
