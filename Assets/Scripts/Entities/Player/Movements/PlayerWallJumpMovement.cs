using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpMovement : PlayerMovementType
{
    [Header("Wall Jump Variables")]
    [SerializeField] private LayerMask wallJumpLayer;
    [SerializeField] private Vector2 JumpOffVelocity = new Vector2(3, 8);
    [SerializeField, Range(0, 8f)]
    private float slideSpeed = 2f;

    [SerializeField, Range(0, 0.5f)]
    private float dropLeeway = 0.125f;
    float dropTimer;

    [Header("Checking Variables")]
    [Range(2, 4)]
    public int raycastChecks = 3;
    public float raycastRadius = 0.125f;
    int requiredSuccessfulCasts;


    float slideMovement = 0;
    bool canGrabWall;

    private void Start()
    {
        canGrabWall = true;
        requiredSuccessfulCasts = Mathf.Clamp((raycastChecks / 2) + 1, 1, int.MaxValue);
        dropTimer = dropLeeway;
    }

    public override bool ShouldUseMovement()
    {
        if (playerMovement.Controller.IsGrounded)
        {
            canGrabWall = true;
            slideMovement = 0;
        }

        if (!canGrabWall) return false;

        int dir = (playerMovement.MoveDirection.x < 0) ? -1 : 1;

        int successfuls = 0;

        List<float> heights = new List<float>();
        float heightDif = playerMovement.Controller.Height - (raycastRadius * 2f);
        heightDif /= (float)raycastChecks;
        for (int i = 0; i < raycastChecks; i++)
            heights.Add((heightDif * i) + raycastRadius);

        float rayDistance = playerMovement.Controller.Radius - raycastRadius + 0.05f;
        foreach(float h in heights)
        {
            Vector2 direction = Vector2.right * dir;
            Vector2 pos = playerMovement.transform.position;
            pos.y += h;

            RaycastHit2D hit = Physics2D.CircleCast(pos, raycastRadius, direction, rayDistance, wallJumpLayer);
            Debug.DrawRay(pos, direction * rayDistance);
            if (hit.collider)
                successfuls++;
        }

        return (successfuls >= requiredSuccessfulCasts);
    }

    public override void Movement(Vector2 input, ref Vector2 moveDirection, bool grounded)
    {
        moveDirection.y = slideMovement;
        slideMovement -= slideSpeed * Time.deltaTime;

        if ((moveDirection.x < 0 && input.x > 0) || (moveDirection.x > 0 && input.x < 0))
            dropTimer -= Time.deltaTime;
        else if (input.y < 0)
            dropTimer -= Time.deltaTime * 2f;
        else if (dropTimer < dropLeeway)
            dropTimer += Time.deltaTime;

        if(dropTimer <= 0)
        {
            canGrabWall = false;
            dropTimer = dropLeeway;
            moveDirection.x = 0;
        }
    }

    public override void FixedMovement(ref Vector2 moveDirection, bool grounded)
    {
    }

    public override void Jump(ref Vector2 moveDirection, bool grounded)
    {
        int dir = (moveDirection.x < 0) ? -1 : 1;
        moveDirection.x = JumpOffVelocity.x * -dir;
        moveDirection.y = JumpOffVelocity.y;
        playerMovement.FreezeMovement();
        dropTimer = dropLeeway;
        slideMovement = 0;
    }
}
