using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    [Tooltip("This is the bottom-left corner of the area")]
    public Vector2 origin = Vector2.zero;
    public Vector2 size = Vector2.one;
    public Vector2 HalfSize => (size / 2f);
    public Vector2 Bounds => new Vector2(origin.x + size.x, origin.y + size.y);
    public Vector2 Center => new Vector2(origin.x + HalfSize.x, origin.y + HalfSize.y);

    public Area()
    {
        origin = Vector2.zero;
        size = Vector2.one;
    }

    public Area(Vector2 o, Vector2 s)
    {
        origin = o; size = s;
    }

    public void SetOriginFromCenter(Vector2 c)
    {
        origin = new Vector2(c.x - HalfSize.x, c.y - HalfSize.y);
    }

    public Vector2 ClampToArea(Vector2 p)
    {
        //Clone the inputted Vector2 to not mess with the one given
        Vector2 cloned = new Vector2(p.x, p.y);
        ClampToArea(ref cloned);
        return cloned;
    }

    public void ClampToArea(ref Vector2 p)
    {
        p.x = Mathf.Clamp(p.x, origin.x, Bounds.x);
        p.y = Mathf.Clamp(p.y, origin.y, Bounds.y);
    }

    public bool IsInArea(Vector2 p)
    {
        return HorizontalCheck(p.x) && VerticalCheck(p.y);
    }

    public bool HorizontalCheck(float val)
    {
        return (val > origin.x && val < Bounds.x);
    }
    public bool VerticalCheck(float val)
    {
        return (val > origin.y && val < Bounds.y);
    }

    public bool CheckOverlapX(Area a)
    {
        return (HorizontalCheck(a.origin.x) || HorizontalCheck(a.Bounds.x));
    }
    public bool CheckOverlapY(Area a)
    {
        return (VerticalCheck(a.origin.y) || VerticalCheck(a.Bounds.y));
    }

    public bool CheckOverlap(Area a)
    {
        return CheckOverlapX(a) && CheckOverlapY(a);
    }

    public bool IsInArea(Area a)
    {
        return CheckOverlap(a) || a.CheckOverlap(this);
    }

    public static bool IsInArea(Area a, Area b)
    {
        return b.CheckOverlap(a) || a.CheckOverlap(b);
    }

    /// <summary>
    /// Do not allow an area to overlap with this one
    /// Adjust the reference to move it out of the bounds of this area
    /// </summary>
    /// <param name="repel">The area that will not overlap this one</param>
    public void RepelArea(ref Area repel)
    {
        //Do nothing if they are not overlapping
        if (!IsInArea(repel)) return;

        Vector2 o = ClampToArea(repel.origin);
        Vector2 b = ClampToArea(repel.Bounds);
        Area overlap = new Area(o, (b - o));
        
        if(overlap.size.y >= overlap.size.x)
        {
            if (repel.Center.x <= Center.x)
                repel.origin.x = Mathf.Clamp(repel.origin.x, float.MinValue, origin.x - repel.size.x);
            else
                repel.origin.x = Mathf.Clamp(repel.origin.x, Bounds.x + 0.01f, float.MaxValue);
        }
        else
        {
            if (repel.Center.y <= Center.y)
                repel.origin.y = Mathf.Clamp(repel.origin.y, float.MinValue, origin.y - repel.size.y);
            else
                repel.origin.y = Mathf.Clamp(repel.origin.y, Bounds.y + 0.01f, float.MaxValue);
        }
    }

    public void DrawGizmo()
    {
        Gizmos.DrawCube(Center, new Vector3(size.x, size.y, 0.05f));
    }
}
