using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RigidbodyController))]
public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 4f;
    public float MoveSpeed => moveSpeed;

    [SerializeField] protected float gravity = 20f;
    
    protected RigidbodyController controller;
    public RigidbodyController Controller => controller;

    protected Vector2 moveDirection;
    public Vector2 MoveDirection => moveDirection;


    protected virtual void Start()
    {
        GetRequiredComponents();
    }
    
    protected virtual void GetRequiredComponents()
    {
        controller = GetComponent<RigidbodyController>();
    }

    protected virtual void FixedUpdate()
    {
        if (!controller.IsGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = Mathf.Clamp(moveDirection.y, 0, float.MaxValue);

        moveDirection = controller.Move(moveDirection, true);
    }
}
