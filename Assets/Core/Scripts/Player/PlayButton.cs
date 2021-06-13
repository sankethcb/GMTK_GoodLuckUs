using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SceneManagement;
public class PlayButton : MonoBehaviour
{
    public SCENES gameScene;
    public void Play()
    {
        gameScene.Load();
    }
}
