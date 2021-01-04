using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public SpriteRenderer rend;
    [SerializeField] public Vector3 target;
    private Vector3 start;
    [SerializeField] float speed = 2f;
    private Vector3 lastPos;
    private bool isGoingLeft = true;
    [SerializeField] public int damage = 1;

    void Start()
    {
        start = transform.position;
        lastPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(start, target, Mathf.PingPong(Time.time * speed, 1.0f));
        if (isGoingLeft && (lastPos.x < transform.position.x))
        {
            isGoingLeft = false;
            rend.flipX = true;
        } else if (!isGoingLeft && (lastPos.x > transform.position.x))
        {
            isGoingLeft = true;
            rend.flipX = false;
        }
        lastPos = transform.position;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (start == new Vector3(0, 0, 0))
            Gizmos.DrawLine(transform.position, target);
        else
            Gizmos.DrawLine(start, target);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmgObj = null;
        if ((dmgObj = col.GetComponent<Damageable>()) != null)
        {
            PlayerDamage player = null;
            if ((player = (dmgObj as PlayerDamage)) != null)
                player.SetKnockback(new Vector2(player.transform.position.x - transform.position.x, 1f));
            dmgObj.Hurt(damage);
        }
    }
}
