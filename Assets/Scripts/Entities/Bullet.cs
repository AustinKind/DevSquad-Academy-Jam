using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PooledObjectRef poolRef;
    private AudioController audioController;

    void Start()
    {
        poolRef = GetComponent<PooledObjectRef>();
        audioController = AudioController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmgObj = null;
        if ((dmgObj = collision.GetComponent<Damageable>()) != null)
        {
            dmgObj.Hurt(5);
        }
        audioController.PlaySound("hit");
        if (poolRef) poolRef.PlaceBackInPool();
    }
}
