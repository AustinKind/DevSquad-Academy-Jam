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

    private bool grounded = false;
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

    float HeightToStepOffset(float distance)
    {
        //Get how much y-difference needed to adjust by to be at the offset
        return (ActualHeight() - distance);
    }
    
    bool CheckForGround(ref RaycastHitInfo hitInfo)
    {
        Vector3 pos = col.bounds.center; //Send from center of collider
        Vector3 dir = Vector3.down; //Shoot towards floor
        float dis = ActualHeight() + 0.02f;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, dis, collisionLayer);
        if (hitInfo.hit = hit)
        {
            hitInfo.hitDistance = hit.distance;
            hitInfo.hitPoint = hit.point;
            hitInfo.hitNormal = hit.normal;
        }

        Debug.DrawRay(pos, dir * dis, (hitInfo.hit) ? Color.green : Color.red);
        return hitInfo.hit;
    }

    Vector2 StepOffsetVelocity()
    {
        //Get the needed velocity to not fall below the step offset
        Vector2 vel = Vector3.zero;
        hitInfo = new RaycastHitInfo();

        grounded = CheckForGround(ref hitInfo);
        if (!grounded)
            return vel;

        vel.y = HeightToStepOffset(hitInfo.hitDistance) / Time.fixedDeltaTime;
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
    
    public void Move(Vector2 vel)
    {
        //Move the rigidbody using velocity while keeping the step offset from the ground
        rBody.velocity = vel + StepOffsetVelocity();
    }
}
