using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidbodyController))]
public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 4f;
    [SerializeField] protected float gravity = 20f;
    protected RigidbodyController controller;

    protected Vector2 moveDirection;
    protected float verticalVelocity = 0;

    private void Start()
    {
        GetRequiredComponents();
    }
    private void GetRequiredComponents()
    {
        controller = GetComponent<RigidbodyController>();
    }

    protected virtual void FixedUpdate()
    {
        if (!controller.IsGrounded)
            verticalVelocity -= gravity * Time.deltaTime;
        else
            verticalVelocity = Mathf.Clamp(verticalVelocity, 0, float.MaxValue);

        moveDirection.y = verticalVelocity;
        controller.Move(moveDirection);
    }
}
