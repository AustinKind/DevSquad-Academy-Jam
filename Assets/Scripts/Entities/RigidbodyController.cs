using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Will be a CharacterController like controller for rigidbody components
///  Adds support for more control and extra features not available when just using rigidbodies for movement
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class RigidbodyController : MonoBehaviour
{
    //Which layers should be used for collision finding
    [SerializeField] private LayerMask collisionLayer;

    [SerializeField] private float height = 2f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField]  private Vector2 offset = Vector2.zero;
    [SerializeField, Range(0.0f, 1.0f)] private float stepOffset = 0.3f;
    [SerializeField, Range(0, 90)] private int slopeLimit = 60;

    public bool grounded = false;
    public bool IsGrounded => grounded;

    RaycastHitInfo hitInfo;

    //Components
    Rigidbody2D rBody;
    CapsuleCollider2D col;

    float ActualHeight()
    {
        //Get the actual height that is to the floor
        return (height - stepOffset) / 2f + stepOffset;
    }

    float HeightToStepOffset()
    {
        //Get how much y-difference needed to adjust by to be at the offset
        float actualRadius = (col.size.x / 2f) - 0.05f;
        float xDif = hitInfo.hitPoint.x - col.bounds.center.x;
        float xDifPercentage = xDif / actualRadius;
        float adjust = -Mathf.Sqrt(1 - Mathf.Pow(xDifPercentage, 2)) + 1;
        adjust *= actualRadius;

        return ActualHeight() - (hitInfo.hitDistance + adjust);
    }
    
    bool CheckForGround(ref RaycastHitInfo hitInfo)
    {
        Vector3 pos = col.bounds.center; //Send from center of collider
        Vector3 dir = Vector2.down; //Shoot towards floor

        float actualRadius = (col.size.x / 2f) - 0.05f;
        float rayDis = ActualHeight() - actualRadius + 0.05f;
        RaycastHit2D hit = Physics2D.CircleCast(pos, actualRadius, dir, rayDis, collisionLayer);
        if (hitInfo.hit = hit.collider)
        {
            hitInfo.hitPoint = hit.point;

            Vector2 rayPos = hit.point + (Vector2.up * 0.025f);

            RaycastHit2D rayHit = Physics2D.Raycast(rayPos, dir, 0.05f, collisionLayer);
            Debug.DrawRay(rayPos, dir * 0.05f, Color.blue);
            hitInfo.hitNormal = (rayHit.collider) ? rayHit.normal : Vector2.up;

            Vector2 playerPos = pos; 
            playerPos.x = hit.point.x;

            hitInfo.hitDistance = Vector2.Distance(playerPos, hit.point);
        }

        Color hitColor = (hitInfo.hit) ? Color.green : Color.red;
        Vector3 circlePos = pos + (dir * rayDis);
        Debug.DrawRay(pos, dir * rayDis, hitColor);
        Debug.DrawRay(circlePos, dir * actualRadius, hitColor);
        Debug.DrawRay(circlePos, Vector2.right * actualRadius, hitColor);
        Debug.DrawRay(circlePos, Vector2.left * actualRadius, hitColor);
        return hitInfo.hit;
    }

    Vector2 StepOffsetVelocity()
    {
        //Get the needed velocity to not fall below the step offset
        Vector2 vel = Vector2.zero;
        hitInfo = new RaycastHitInfo();

        grounded = CheckForGround(ref hitInfo);
        if (!grounded)
            return vel;

        vel.y = HeightToStepOffset() / Time.fixedDeltaTime;
        return vel;
    }

    /// <summary>
    ///  Called anytime a change has been made to this component's variables in the inspector
    /// </summary>
    private void OnValidate()
    {
        GetComponents();
        ForceRigidbodyCompliance();
        ValidateColliderValues();
        UpdateColliderBounds();
    }

    /// <summary>
    ///  Get the required components fot the script
    /// </summary>
    void GetComponents()
    {
        rBody = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }


    /// <summary>
    ///  Make sure the rigidbody is set correctly for this type of controlled movement
    /// </summary>
    void ForceRigidbodyCompliance()
    {
        rBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        rBody.freezeRotation = true;
        rBody.gravityScale = 0;

        rBody.sharedMaterial = Resources.Load<PhysicsMaterial2D>("NoFriction");
    }

    /// <summary>
    ///  Used to make sure some inputs for the collider do not make it unusable
    ///  Ex. Step Offset cannot be bigger than the height
    /// </summary>
    void ValidateColliderValues()
    {
        height = Mathf.Clamp(height, 0.02f, float.MaxValue);
        radius = Mathf.Clamp(radius, 0.01f, col.size.y / 2f);

        stepOffset = Mathf.Clamp(stepOffset, 0, height - (radius * 2f));
    }

    /// <summary>
    ///  Make changes to the connect collider component based on the inspector variables' values
    /// </summary>
    void UpdateColliderBounds()
    {
        col.offset = new Vector2(offset.x, ((height + stepOffset) / 2f) + offset.y);
        col.size = new Vector2(radius * 2f, height - stepOffset);
    }
    
    public Vector2 Move(Vector2 vel)
    {
        Vector2 moveVel = vel;
        //Adjust the movement to stick to the ground (slopes)
        if (grounded)
        {
            //Get the angle of the ground
            Vector2 groundAngle = new Vector2(Mathf.Sign(hitInfo.hitNormal.x) * hitInfo.hitNormal.y, -(Mathf.Sign(hitInfo.hitNormal.x) * hitInfo.hitNormal.x));
            float angle = Mathf.Abs(Mathf.Atan2(hitInfo.hitNormal.x, hitInfo.hitNormal.y) * Mathf.Rad2Deg);

            if (groundAngle.x < 0) groundAngle = -groundAngle; //Force the angle to be positive
            Debug.DrawRay(transform.position, groundAngle, Color.blue);

            //Remove left and right velocity
            vel.x = 0;
            vel += (groundAngle * moveVel.x);
        }
        else if(vel.y > 0)
        {
            Vector3 pos = col.bounds.center; //Send from center of collider

            float actualRadius = (col.size.x / 2f) - 0.05f;
            float rayDis = ActualHeight() - actualRadius - stepOffset + 0.05f;
            RaycastHit2D hit = Physics2D.CircleCast(pos, actualRadius, Vector2.up, rayDis, collisionLayer);
            if (hit.collider)
                vel.y = moveVel.y = 0;
        }

        //Move the rigid body using velocity while keeping the step offset from the ground
        rBody.velocity = vel + StepOffsetVelocity();
        return moveVel;
    }
}
