using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2Helper : MonoBehaviour
{
    public static Vector2 Raw(Vector2 adjust)
    {
        Vector2 r = Vector2.zero;
        if (adjust.x > 0) r.x = 1;
        else if (adjust.x < 0) r.x = -1;

        if (adjust.y > 0) r.y = 1;
        else if (adjust.y < 0) r.y = -1;
        return r;
    }
}

