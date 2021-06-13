using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SceneManagement;

public class LoopingMusic : MonoBehaviour
{
    public AudioSource Music;
    static LoopingMusic instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneHandler.OnSceneLoaded += RestartMusic;
    }

    void OnDestroy()
    {
        if (instance != this)
            return;

        instance = null;
        SceneHandler.OnSceneLoaded -= RestartMusic;
    }

    void RestartMusic(SCENES scene)
    {
        if (scene != SCENES.Menu)
            return;

        Music.Stop();
        Music.Play();
    }
}
