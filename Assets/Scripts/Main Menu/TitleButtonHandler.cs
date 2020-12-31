using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonHandler : MonoBehaviour {

    public GameObject panel;
    public GameObject loadingScreenPanel;
    public GameObject newGameBtn;
    public GameObject settingsBtn;
    public GameObject exitBtn;
    public GameObject cam;

    bool loading = false;

    public void StartGame()
    {
        if(!loading)
            StartCoroutine(NewGame());
    }

    /// <summary>
    /// So Async operations should always be in a IEnumerator because the amount of frames it could take will vary depending on size of scene and computer
    /// This way we do each operation individually so we dont load the level until the player is loaded in
    /// Unloading the menu at the end makes it easy to add a loading screen and makes it so the game isnt visable before it is fully loaded
    /// Also if you unload the menu (say load the player scene without additive as well since that forces an unload on all current scenes) the game may not load properly due to this script being in the menu scene
    /// We should have a Singleton doing all scene management for protection of continuity but for now this should work fine
    /// </summary>
    IEnumerator NewGame() 
    {
        loading = true;
        loadingScreenPanel.SetActive(true);
        AsyncOperation loadPlayer = SceneManager.LoadSceneAsync("Load Player", LoadSceneMode.Additive);
        while (!loadPlayer.isDone)
            yield return null;
        cam.GetComponent<AudioListener>().enabled = false;
        AsyncOperation loadGame = SceneManager.LoadSceneAsync("Game Scene 1", LoadSceneMode.Additive);
        while (!loadGame.isDone)
            yield return null;

        AsyncOperation unloadMenu = SceneManager.UnloadSceneAsync(0);
        while (!unloadMenu.isDone)
            yield return null; 
    }


    public void Settings() {
        panel.SetActive(true);
        newGameBtn.SetActive(false);
        settingsBtn.SetActive(false);
        exitBtn.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    public void SettingsBack() {
        panel.SetActive(false);
        newGameBtn.SetActive(true);
        settingsBtn.SetActive(true);
        exitBtn.SetActive(true);
    }
}
