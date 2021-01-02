using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    [SerializeField] public string trackName;
    private AudioController audioController;

    void Awake()
    {
        audioController = AudioController.Instance;
    }

    void Start()
    {
        audioController.PlaySong(trackName);
    }
}
