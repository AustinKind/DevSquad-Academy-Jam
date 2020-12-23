using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float distance = 1f;
    [Range(0.0f, 1.0f)]
    public float radius = 0.25f;

    public void Update()
    {
        float d = Mathf.Clamp(distance - radius, 0f, Mathf.Max(distance, radius));
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.down, d);

        Color hitColor = (hit) ? Color.green : Color.red;
        Debug.DrawRay(transform.position + (Vector3)(Vector2.down * d), Vector2.right * radius, hitColor);
        Debug.DrawRay(transform.position + (Vector3)(Vector2.down * d), Vector2.left * radius, hitColor);
        Debug.DrawRay(transform.position, Vector2.down * d, hitColor);
    }
}
