using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollectible : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer Instruction;

    public Transform SpriteTransform => animator.transform;



    public void HighFivePlayer()
    {
        animator.SetTrigger("Highfive");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<CollectibleHandler>().SetCollectible(this);

        Instruction.transform.DOLocalMoveY(1, 0.2f).SetEase(Ease.OutCubic);
        FadeInstruction(1);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.GetComponent<CollectibleHandler>().SetCollectible(null);

        Instruction.transform.DOLocalMoveY(0.5f, 0.2f).SetEase(Ease.OutCubic);
        FadeInstruction(0);
    }

    public void FadeInstruction(float fade)
    {
        Instruction.DOFade(fade, 0.2f).SetEase(Ease.OutCubic);
    }
}
