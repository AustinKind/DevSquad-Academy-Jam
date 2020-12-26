using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Animator animator;

    // How long the spikes are retracted for.
    [SerializeField] private float idleTime = 4;
    // How long the spikes are in the ready state.
    [SerializeField] private float readyTime = 1;
    // How long the spikes are up for.
    [SerializeField] private float impaledTime = 2;
    // How much damage the spikes do.
    [SerializeField] private int damage = 1;

    void Start ()
    {
        StartCoroutine("SpikeCycle");
    }

    IEnumerator SpikeCycle ()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            animator.SetTrigger("Ready");
            yield return new WaitForSeconds(readyTime);
            animator.SetTrigger("Impale");
            yield return new WaitForSeconds(impaledTime);
            animator.SetTrigger("Retract");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmgObj = null;
        if ((dmgObj = col.GetComponent<Damageable>()) != null)
        {
            if(dmgObj as PlayerDamage)
                dmgObj.Hurt(damage);
        }
    }
}
