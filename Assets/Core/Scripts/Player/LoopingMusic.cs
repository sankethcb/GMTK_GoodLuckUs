using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SceneManagement;

public class LoopingMusic : MonoBehaviour
{
    public AudioSource Music;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneHandler.OnSceneLoaded += RestartMusic;
    }

    void RestartMusic(SCENES scene)
    {
        if (scene != SCENES.Menu)
            return;

        Music.Stop();
        Music.Play();
    }
}
