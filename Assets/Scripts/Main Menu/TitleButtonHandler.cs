﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonHandler : MonoBehaviour {

    public GameObject panel;
    public GameObject newGameBtn;
    public GameObject settingsBtn;
    public GameObject exitBtn;
    public GameObject cam;

    bool loading = false;

    public void StartGame()
    {
        if (!loading)
        {
            loading = true;
            GameSceneController.Instance.StartGame();
        }
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
