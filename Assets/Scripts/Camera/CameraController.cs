using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Variables")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 cameraOffset = Vector2.up;
    [SerializeField] private FloatRange strengthRange = new FloatRange(3, 6);
    [Range(0.125f, 1.0f), SerializeField] private float minStrength = 0.125f;
    [SerializeField] private float cameraFollowSpeed = 12f;

    [Header("Camera Areas")]
    [SerializeField] private Area followBounds;
    [SerializeField] private Area[] collisionBounds;

    Vector3 followPos => target.position + (Vector3)cameraOffset;

    KeyValuePair<float, float> cameraInfo;
    public Area cameraBounds;
    Camera cam;

    public float strength = 0.5f;

    private void OnValidate()
    {
        GetRequiredComponents();

        if (target != null)
            followBounds.SetOriginFromCenter(followPos);

        UpdateCamera();

        foreach (Area bound in collisionBounds)
            bound.RepelArea(ref cameraBounds);
    }

    private void GetRequiredComponents()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void UpdateCamera()
    {
        if (cameraInfo.Key == cam.orthographicSize && cameraInfo.Value == cam.aspect) return;

        float height = 2f * cam.orthographicSize;
        Vector2 camSize = new Vector2(height * cam.aspect, height);
        cameraBounds.size = camSize;
        cameraInfo = new KeyValuePair<float, float>(cam.orthographicSize, cam.aspect);
    }


    private void Start()
    {
        GetRequiredComponents();
        UpdateCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        followBounds.SetOriginFromCenter(followPos);

        Vector2 adjustDir = Vector2.zero;
        if (!followBounds.IsInArea(transform.position))
            adjustDir = (followBounds.ClampToArea(transform.position) - (Vector2)transform.position);

        strength = CalculateStrength(adjustDir.magnitude);
        cameraBounds.SetOriginFromCenter((Vector2)transform.position + adjustDir * strength);
        foreach (Area bound in collisionBounds)
            bound.RepelArea(ref cameraBounds);

        transform.position = Vector3.SlerpUnclamped(transform.position, cameraBounds.Center, Time.deltaTime * cameraFollowSpeed);
    }

    public float CalculateStrength(float dis)
    {
        float s = dis;
        strengthRange.ClampToRange(ref s);
        s -= strengthRange.min; s /= strengthRange.Difference;
        return Mathf.Clamp(s + minStrength, 0f, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0.2f, 0.125f);
        followBounds.DrawGizmo();

        Gizmos.color = new Color(1, 0, 0, 0.25f);
        for (int i = 0; i < collisionBounds.Length; i++)
            collisionBounds[i].DrawGizmo();

        Gizmos.color = new Color(0, 0, 1, 0.125f);
        cameraBounds.DrawGizmo();
    }
}