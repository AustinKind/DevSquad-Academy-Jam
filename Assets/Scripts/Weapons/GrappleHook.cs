using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleHook : Gun
{
    [SerializeField] private Transform hookTransform;
    [SerializeField] private LayerMask hookSpotLayer;
    [SerializeField] private Vector2 hookOffset;
    [SerializeField] private AnimationCurve hookTravel;
    [SerializeField, Range(1f, 5f)] private float grappleLength = 1.5f;
    [SerializeField, Range(0.25f, 2f)] private float timeTillHookReturn = 1f;

    Vector2 grappleDir;
    LineRenderer grappleLine;

    float hookTime = 0f;
    bool grappling = false;
    bool hooked = false;
    public bool IsHooked => hooked;
    public float GrappleLength => grappleLength;
    public Transform HookTransform => hookTransform;
    public Vector2 HookOffset => hookOffset;

    public override void Start()
    {
        base.Start();
        hookTransform.gameObject.SetActive(false);
        GetRequiredComponents();

        grappleLine.positionCount = 2;
        grappleLine.SetPosition(0, hookOffset);
    }

    void GetRequiredComponents()
    {
        grappleLine = GetComponent<LineRenderer>();
    }

    public override void Shoot(Vector2 dir)
    {
        if (grappling || !canShootGun) return;
        grappleDir = dir;

        hookTime = 0f;
        hooked = false;
        canShootGun = false;
        hookTransform.gameObject.SetActive(grappling = true);
    }
    public override void Reload()
    {
    }

    public void Unhook(bool shot)
    {
        if (shot)
            StartCoroutine(RegisterShot());
        else canShootGun = true;

        hooked = false;
        hookTransform.SetParent(transform);
        hookTransform.localPosition = hookOffset;

        grappleLine.SetPosition(1, hookOffset);
        hookTransform.gameObject.SetActive(grappling = false);
    }

    private void Update()
    {
        grappleLine.SetPosition(1, transform.InverseTransformPoint(hookTransform.position));

        if (grappling)
        {
            float adjust = grappleLength * hookTravel.Evaluate(hookTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(hookOffset), grappleDir, adjust, hookSpotLayer);
            if (hit.collider)
            {
                hookTransform.SetParent(hit.collider.transform);
                hookTransform.localPosition = Vector3.zero;
                hooked = true;
            }

            if (hooked) return;

            hookTransform.localPosition = hookOffset + (grappleDir * adjust);

            hookTime = Mathf.Clamp(hookTime + (Time.deltaTime / timeTillHookReturn), 0f, 1f);
            if (hookTime >= 1f)
            {
                StartCoroutine(RegisterShot());
                hookTransform.gameObject.SetActive(grappling = false);
            }
        }
    }
}
