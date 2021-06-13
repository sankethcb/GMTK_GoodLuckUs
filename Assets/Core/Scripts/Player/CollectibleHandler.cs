using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Input;
using DG.Tweening;
using UnityEngine.InputSystem;
public class CollectibleHandler : MonoBehaviour
{
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;

    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMovement2D playerMovement;
    [SerializeField] PlayerAnimator2D playerAnimator;
    [SerializeField] InputEventSystem input;

    PlayerCollectible m_collectible;

    public void Interact(InputAction.CallbackContext inputCallback)
    {
        StartCollection();
    }


    void StartCollection()
    {
        playerAnimator.Wave();
        input.enabled = false;
        playerMovement.enabled = false;

        if (m_collectible == null)
        {
            input.enabled = true;
            playerMovement.enabled = true;
            return;
        }

        StartCoroutine(HandleCollection());
    }

    void EndCollection()
    {
        input.enabled = true;
        playerMovement.enabled = true;

        playerData.GroupCount++;
        Destroy(m_collectible.gameObject);
        m_collectible = null;
    }

    public void SetCollectible(PlayerCollectible collectible) => m_collectible = collectible;

    IEnumerator HandleCollection()
    {
        m_collectible.FadeInstruction(0);

        Vector2 lockInPos = rightPos.position;
        string dir = "Right";
        if (m_collectible.transform.position.x < transform.position.x)
        {
            lockInPos = leftPos.position;
            m_collectible.SpriteTransform.localScale = new Vector3(-1, 1, 1);
            dir = "Left";
        }

        m_collectible.transform.DOMove(lockInPos, 2.0f).SetEase(Ease.OutExpo);
        yield return m_collectible.transform.DOScale(1.2f, 2.0f).SetEase(Ease.OutExpo).WaitForCompletion();

        playerAnimator.HighFive(dir);
        m_collectible.HighFivePlayer();

        yield return new WaitForSeconds(1.2f);
        m_collectible.SpriteTransform.DOMove(transform.position, 0.5f).SetEase(Ease.OutExpo);
        yield return m_collectible.SpriteTransform.DOScale(0, 0.5f).SetEase(Ease.OutExpo).WaitForCompletion();

        playerAnimator.Wave();

        EndCollection();
    }

}
