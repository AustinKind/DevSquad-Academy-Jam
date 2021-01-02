using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioController : MonoBehaviour
{
    static AudioController instance = null;
    [SerializeField] Sound[] sounds;
    private string currentSong = null;

    public static AudioController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (AudioController)FindObjectOfType(typeof(AudioController));
                if (instance == null)
                    instance = (new GameObject("AudioController")).AddComponent<AudioController>();
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

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioController: Sound not found: " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        Debug.LogWarning("AudioController: Sound not found: " + _name);
    }

    public void PlaySong(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (currentSong != null)
                    StopSound(currentSong);
                sounds[i].Play();
                currentSong = _name;
                return;
            }
        }
        Debug.LogWarning("AudioController: Sound not found: " + _name);
    }
}
