using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappledMovement : PlayerMovementType
{
    [SerializeField] private FloatRange swingSpeed = new FloatRange(4, 6);
    [SerializeField] private float jumpVelocity = 8f;
    [SerializeField] private float minimumSwingRadius = 0.25f;

    GrappleHook grapple;
    Vector2 difference;
    bool startedMoving = false;
    bool flipped = false;
    float speed;
    float dis;

    private void Start()
    {
        grapple = GetComponentInChildren<GrappleHook>();
    }

    public override void Jump(ref Vector2 moveDirection, bool grounded)
    {
        moveDirection.y = jumpVelocity;
        startedMoving = false;
        grapple.Unhook(true);
    }

    public override bool ShouldUseMovement()
    {
        if (grapple == null) return false;

        bool grappled = grapple.IsHooked;
        if(grappled && !startedMoving)
        {
            Vector2 dif = (transform.position - grapple.HookTransform.position);
            difference.x = dif.x;
            flipped = (difference.x >= 0);
            dis = Mathf.Clamp(dif.magnitude, minimumSwingRadius, grapple.GrappleLength);

            float disPercent = (dis - minimumSwingRadius) / (grapple.GrappleLength - minimumSwingRadius);
            speed = swingSpeed.EvaluatePercent(disPercent);
            Debug.Log(speed);
            startedMoving = true;
        }

        return grappled;
    }

    public override void FixedMovement(ref Vector2 moveDirection, bool grounded)
    {
        //TODO
        int direction = (flipped) ? -1 : 1;

        float adjust = (direction * Time.fixedDeltaTime * speed);
        float percent = Mathf.Clamp(difference.x / dis, -1f, 1f);
        difference.y = -Mathf.Sqrt(1 - Mathf.Pow(percent, 2)) + 1;

        Vector2 offset = new Vector2(percent, difference.y) * dis;
        offset -= Vector2.up * dis;

        adjust *= Mathf.Pow(Mathf.Clamp(Mathf.Abs(difference.y - 1f), 0.1f, 1f), 2);
        difference.x = Mathf.Clamp(difference.x + adjust, -dis, dis);

        if (flipped && difference.x <= -dis && difference.y >= 1f) flipped = false;
        else if (!flipped && difference.x >= dis && difference.y >= 1f) flipped = true;

        offset -= grapple.HookOffset;
        Vector3 moveTo = (grapple.HookTransform.position + (Vector3)offset);
        Debug.DrawLine(transform.position, moveTo, Color.red);
        //PLACEHOLDER MOVEMENT
        moveDirection = (moveTo - transform.position) / (Time.fixedDeltaTime * speed);
    }

}
