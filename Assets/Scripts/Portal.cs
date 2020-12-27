using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerEnter2D (Collider2D col)
    {
        if (col.CompareTag("Player"))
            isTriggered = true;
    }

    private void OnTriggerExit2D (Collider2D col)
    {
        if (col.CompareTag("Player"))
            isTriggered = false;
    }

    void Update ()
    { 
        // If player is on top of portal and clicks up, teleport them to next level
        if (isTriggered && Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            StartCoroutine("NextLevel");      
        }
    }

    IEnumerator NextLevel()
    {
        // TODO: Determine what scene to load based on player progress
        // TODO: Keep track of what game scene is currently loaded
        
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Game Scene 2", LoadSceneMode.Additive);
        while (!loadScene.isDone)
            yield return null;
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync("Game Scene 1");
        while (!unloadScene.isDone)
            yield return null;
    }
}
