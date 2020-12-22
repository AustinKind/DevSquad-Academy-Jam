using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isTriggered = false;
    private GameObject player;

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.tag == "Player")
            isTriggered = true;
    }

    private void OnTriggerExit2D (Collider2D collider)
    {
        if (collider.tag == "Player")
            isTriggered = false;
    }

    void Start ()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update ()
    { 
        // If player is on top of portal and clicks up, teleport them to next level
        if (isTriggered && Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            // Add code to determine what scene to load based on player progress
            SceneManager.UnloadSceneAsync("Game Scene 1");
            player.transform.position = new Vector3(-7.5f, -2.36f, 0);
            SceneManager.LoadSceneAsync("Game Scene 2", LoadSceneMode.Additive);
        }
    }
}
