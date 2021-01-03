using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPlayerExit : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        StartCoroutine("Enter");
    }

    IEnumerator Enter()
    {
        yield return new WaitForSeconds(1);
        anim.enabled = true;
    }
}
