using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    [SerializeField] public string trackName;
    [SerializeField] public float delay = 0.2f;
    private AudioController audioController;

    void Awake()
    {
        audioController = AudioController.Instance;
    }

    void Start()
    {
        StartCoroutine("Delay");
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        audioController.PlaySong(trackName);
    }
}
