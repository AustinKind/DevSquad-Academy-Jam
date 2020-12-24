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
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = Mathf.Clamp(moveDirection.y, 0, float.MaxValue);

        moveDirection = controller.Move(moveDirection);
    }
}
