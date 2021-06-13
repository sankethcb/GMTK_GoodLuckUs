using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SceneManagement;


public class LevelExit : MonoBehaviour
{
    public SCENES NextScene;
    void OnTriggerEnter2D(Collider2D other) 
    {
        NextScene.Load();
    }
}
