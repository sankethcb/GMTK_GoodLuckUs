using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip, 0.8f);
    }

    public void PlayClipSoft(AudioClip clip)
    {
        source.PlayOneShot(clip, 0.2f);
    }
}
