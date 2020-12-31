using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombController : MonoBehaviour
{
    private Bomb[] bombs;
    private GameObject player;
    public GameObject cam;

    void Start()
    {
        bombs = GetComponentsInChildren<Bomb>();
        player = PlayerStatusManager.Instance.Player.gameObject;
    }

    public void CheckCompletion ()
    {
        foreach (Bomb bomb in bombs)
        {
            if (bomb.isActivated)
                return;
        }
        // If all bombs are deactivated, then play exit animation then change level
        Debug.Log("All bombs defused!");
        StartCoroutine("ChangeLevel");
    }

    IEnumerator ChangeLevel()
    {
        // Play animation here
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        cam.GetComponent<AudioListener>().enabled = false;
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Game Scene 2", LoadSceneMode.Additive);
        while (!loadScene.isDone)
            yield return null;
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync("Game Scene 1");
        while (!unloadScene.isDone)
            yield return null;
    }
}
