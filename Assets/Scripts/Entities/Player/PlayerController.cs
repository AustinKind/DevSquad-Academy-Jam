using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput input;
    PlayerMovement movement;

    delegate void OnJumpAction();
    OnJumpAction onJump;

    private void Start()
    {
        GetRequiredComponents();
        SetActions();
    }

    private void GetRequiredComponents()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }

    void SetActions()
    {
    }
}
