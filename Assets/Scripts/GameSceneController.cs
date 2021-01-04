using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private float minLoadTime = 0.5f;
    static GameSceneController instance;
    PlayerController player;
    PortalController portal;

    int loadedLevel = 0;

    public static GameSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameSceneController)FindObjectOfType(typeof(GameSceneController));
                if (instance == null)
                    instance = (new GameObject("_GameSceneController")).AddComponent<GameSceneController>();
            }
            return instance;
        }
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameLoad());
        IEnumerator StartGameLoad()
        {
            AsyncOperation loadingScreen = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            while (!loadingScreen.isDone) yield return null;

            AsyncOperation loadPlayer = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            while (!loadPlayer.isDone) yield return null;

            player = PlayerStatusManager.Instance.Player;
            portal = player.gameObject.GetComponent<PortalController>();

            AsyncOperation gameScene = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
            while (!gameScene.isDone) yield return null;

            loadedLevel = 3;
            MoveToSpawnPoint();
            ActivatePlayer();

            yield return new WaitForSeconds(minLoadTime);
            AsyncOperation unloadMenu = SceneManager.UnloadSceneAsync(0);
            while (!unloadMenu.isDone) yield return null;
            AsyncOperation unloadLoading = SceneManager.UnloadSceneAsync(1);
            while (!unloadLoading.isDone) yield return null;
            portal.ExitPortal();
        }
    }

    public void NextLevel()
    {
        int previous = loadedLevel;
        int index = previous + 1;
        if (index >= SceneManager.sceneCountInBuildSettings)
            index = 0; //Go to title screen

        StartCoroutine(NextLevelLoad());
        IEnumerator NextLevelLoad()
        {
            portal.EnterPortal();
            yield return new WaitForSeconds(2.1f);
            DeactivatePlayer();
            AsyncOperation loadingScreen = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            while (!loadingScreen.isDone) yield return null;

            AsyncOperation gameScene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            while (!gameScene.isDone) yield return null;

            loadedLevel = index;
            bool stillPlaying = (loadedLevel != 0);
            if (stillPlaying) MoveToSpawnPoint();

            yield return new WaitForSeconds(minLoadTime);
            AsyncOperation unloadPrevious = SceneManager.UnloadSceneAsync(previous);
            while (!unloadPrevious.isDone) yield return null;

            if (stillPlaying)
                ActivatePlayer();
            else //Back at the main menu
                StartCoroutine(RemovePlayerObjects());

            AsyncOperation unloadLoading = SceneManager.UnloadSceneAsync(1);
            while (!unloadLoading.isDone) yield return null;
            if (stillPlaying)
                portal.ExitPortal();
        }
    }

    public void GameOver()
    {
        int previous = loadedLevel;
        StartCoroutine(GameOverLoad());
        IEnumerator GameOverLoad()
        {
            DeactivatePlayer();
            AsyncOperation loadingScreen = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            while (!loadingScreen.isDone) yield return null;

            AsyncOperation gameScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
            while (!gameScene.isDone) yield return null;

            loadedLevel = 0;

            yield return new WaitForSeconds(minLoadTime);
            Debug.Log("previous: " + previous);
            AsyncOperation unloadPrevious = SceneManager.UnloadSceneAsync(previous);
            while (!unloadPrevious.isDone) yield return null;
            StartCoroutine(RemovePlayerObjects());
            AsyncOperation unloadLoading = SceneManager.UnloadSceneAsync(1);
            while (!unloadLoading.isDone) yield return null;
        }
    }

    IEnumerator RemovePlayerObjects()
    {
        foreach (ObjectPoolParent pools in FindObjectsOfType<ObjectPoolParent>())
            Destroy(pools.gameObject);

        Destroy(TimeScaleManager.Instance.gameObject);
        Destroy(PlayerStatusManager.Instance.gameObject);

        AsyncOperation unloadPlayer = SceneManager.UnloadSceneAsync(2);
        while (!unloadPlayer.isDone) yield return null;
    }

    public void MoveToSpawnPoint()
    {
        if (player == null) return;
        GameObject spawn = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        Vector2 spawnPos = (spawn != null) ? (Vector2)spawn.transform.position : Vector2.zero;
        player.transform.position = spawnPos;
    }

    public void ActivatePlayer()
    {
        if (player == null) return;
        FindObjectOfType<ParalaxBackground>().Initialize();
        player.gameObject.SetActive(true);
    }

    public void DeactivatePlayer()
    {
        if (player == null) return;
        player.onNextLevel.Invoke();
        player.gameObject.SetActive(false);
    }
}
