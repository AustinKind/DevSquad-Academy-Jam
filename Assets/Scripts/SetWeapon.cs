using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeapon : MonoBehaviour
{
    [SerializeField, Range(1,2)] private int setWeaponTo = 2;
    PlayerController playerController;
    GunController gunController;

    void Start()
    {
        playerController = PlayerStatusManager.Instance.Player;
        gunController = playerController.GetComponentInChildren<GunController>();
        gunController.SetWeapon(setWeaponTo);
    }
}
