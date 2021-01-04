using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimationHandler : MonoBehaviour
{
    public GameObject anim1;
    public GameObject anim2;
    public GameObject anim3;
    public GameObject anim4;

    void Start()
    {
        StartCoroutine("AnimationLoop");
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            anim1.SetActive(true);
            yield return new WaitForSeconds(4.2f);
            anim1.SetActive(false);
            anim2.SetActive(true);
            yield return new WaitForSeconds(4.2f);
            anim2.SetActive(false);
            anim3.SetActive(true);
            yield return new WaitForSeconds(4.2f);
            anim3.SetActive(false);
            anim4.SetActive(true);
            yield return new WaitForSeconds(4.2f);
            anim4.SetActive(false);
        }    
    }
}
