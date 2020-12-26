using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Area area1;
    public Area[] block;

    private void OnDrawGizmos()
    {
        bool overlapping = false;
        for (int i = 0; i < block.Length; i++)
        {
            block[i].RepelArea(ref area1);
            Gizmos.color = Area.IsInArea(area1, block[i]) ? new Color(1, 0, 0, 0.125f) : Color.green;
            block[i].DrawGizmo();
        }

        Gizmos.color = overlapping ? new Color(1, 0, 0, 0.125f) : Color.green;
        area1.DrawGizmo();
    }
}
