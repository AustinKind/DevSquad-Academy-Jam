using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject characterSprite;
    public GameObject enterPortal;
    public GameObject exitPortal;

    public void EnterPortal()
    {
        characterSprite.SetActive(false);
        playerController.DisableInput(true);
        enterPortal.SetActive(true);
        StartCoroutine("WaitForEnter");
    }

    public void ExitPortal()
    {
        characterSprite.SetActive(false);
        playerController.DisableInput(true);
        exitPortal.SetActive(true);
        StartCoroutine("WaitForExit");
    }

    IEnumerator WaitForEnter()
    {
        yield return new WaitForSeconds(2.1f);
        enterPortal.SetActive(false);
        playerController.DisableInput(false);
        characterSprite.SetActive(true);
    }

    IEnumerator WaitForExit()
    {
        yield return new WaitForSeconds(1.8f);
        exitPortal.SetActive(false);
        characterSprite.SetActive(true);
        playerController.DisableInput(false);
    }
}
