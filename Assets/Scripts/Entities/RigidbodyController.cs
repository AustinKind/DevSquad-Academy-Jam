using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float height = 2f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField]  private Vector2 offset = Vector2.zero;
    [SerializeField, Range(0.0f, 1.0f)] private float stepOffset = 0.3f;

    //Components
    Rigidbody2D rBody;
    CapsuleCollider2D col;

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
}
