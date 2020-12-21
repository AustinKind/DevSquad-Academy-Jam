using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonHandler : MonoBehaviour {
    
    public void newGame() {
        SceneManager.LoadSceneAsync("Game Scene");
    }

    public void exitGame() {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
