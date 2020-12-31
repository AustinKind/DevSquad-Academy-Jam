using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeScene : MonoBehaviour
{
    void Start()
    {
        GameObject player = PlayerStatusManager.Instance.Player.gameObject;
        player.SetActive(true);
        player.transform.position = transform.position; 
    }
}
