using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeScene : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.SetActive(true);
        player.transform.position = transform.position;
    }
}
