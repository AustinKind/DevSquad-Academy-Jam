using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeapon : MonoBehaviour
{
    PlayerController playerController;
    GunController gunController;

    void Start()
    {
        playerController = PlayerStatusManager.Instance.Player;
        Component[] gunControllers = playerController.gameObject.transform.GetComponentsInChildren<GunController>();
        foreach(Component comp in gunControllers)
        {
            gunController = (GunController)comp;
        }
        Debug.Log(gunController);
        gunController.SetWeapon(2);
    }
}
