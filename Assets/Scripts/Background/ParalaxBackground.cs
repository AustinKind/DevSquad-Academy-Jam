using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField] private ParalaxTransform[] backgrounds;
    [SerializeField] private Vector2 maxDifference = new Vector2(15, 4);

    [SerializeField, Tooltip("This is the area that the camera will be traversing through")] 
    private Area mapArea;

    CameraController cam;
    Area paralaxArea;

    Vector2 ParalaxPercent
    {
        get
        {
            if (paralaxArea == null) return Vector2.zero;
            Vector2 percent = Vector2.zero;
            Vector2 clampedPos = paralaxArea.ClampToArea(cam.cameraBounds.Center);

            Vector2 dif = (clampedPos - paralaxArea.origin);
            percent.x = Mathf.Clamp(dif.x / paralaxArea.size.x, 0f, 1f);
            percent.y = Mathf.Clamp(dif.y / paralaxArea.size.y, 0f, 1f);
            return percent;
        }
    }

    Vector2 ParalaxPosition
    {
        get
        {
            if (paralaxArea == null) return Vector2.zero;
            Vector2 pos = ParalaxPercent;
            pos.x = (pos.x * 2f) - 1f;
            pos.x *= -maxDifference.x;
            pos.y *= maxDifference.y;
            Debug.Log(pos);
            return pos;
        }
    }


    private void Start()
    {
        cam = FindObjectOfType<CameraController>();
        transform.SetParent(cam.transform);

        SetParalaxArea();
        SetParalaxBackgrounds(ParalaxPosition);
    }

    void SetParalaxArea()
    {
        paralaxArea = new Area(mapArea.origin, mapArea.size);
        Vector2 adjust = cam.cameraBounds.size;
        paralaxArea.origin += adjust / 2f;
        paralaxArea.size -= adjust;
    }

    void SetParalaxBackgrounds(Vector2 pos)
    {
        foreach(ParalaxTransform bg in backgrounds)
            bg.background.localPosition = pos * bg.paralaxPercent;
    }

    void LerpParalaxBackgrounds(Vector2 pos)
    {
        foreach (ParalaxTransform bg in backgrounds)
            bg.background.localPosition = Vector3.Lerp(bg.background.localPosition, pos * bg.paralaxPercent, Time.deltaTime * Mathf.Sqrt(maxDifference.magnitude));
    }

    private void LateUpdate()
    {
        LerpParalaxBackgrounds(ParalaxPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.125f);
        mapArea.DrawGizmo();

        if (paralaxArea != null)
            paralaxArea.DrawGizmo();
    }
}

[System.Serializable]
public class ParalaxTransform
{
    public Transform background;
    [Range(0f, 1f)] 
    public float paralaxPercent = 1f;
}