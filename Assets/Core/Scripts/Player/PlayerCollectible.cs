using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollectible : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Transform SpriteTransform => animator.transform;

    public void HighFivePlayer()
    {
        animator.SetTrigger("Highfive");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<CollectibleHandler>().SetCollectible(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.GetComponent<CollectibleHandler>().SetCollectible(null);
    }
}
