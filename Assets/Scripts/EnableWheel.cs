using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWheel : MonoBehaviour
{
    PlayerController playerController;
    GunController gunController;

    void Start()
    {
        playerController = PlayerStatusManager.Instance.Player;
        gunController = playerController.GetComponentInChildren<GunController>();
        gunController.wheelUI = GameObject.Find("Weapon Wheel").GetComponent<Animator>();
    }
}
