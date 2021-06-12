using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerFoldOut : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMovement2D playerMovement;
    [SerializeField] PlayerGroundCheck2D groundCheck;
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform playerSpriteObject;

    [Space()]
    [SerializeField] List<Transform> foldOutTransforms;
    [SerializeField] List<SpriteRenderer> foldOutSprites;

    [Header("Settings")]
    [SerializeField] Sprite foldoutSprite;
    [SerializeField] float foldoutSpeed = 1;
    [SerializeField] float foldoutAcceleration = 1;
    [SerializeField] Vector2 foldoutSize;

    Vector2 m_foldoutPosition;
    Vector2 m_foldoutOffset;
    Vector3 m_foldoutScale = Vector3.one;

    Sequence m_foldoutSequence;

    float m_foldOutTime;

    bool _succesfullFoldout = false;

    void Awake()
    {

    }

    [ContextMenu("Set Sprite")]
    void SetFoldOutSprite()
    {
        for (int i = 0; i < foldOutSprites.Count; i++)
        {
            foldOutSprites[i].sprite = foldoutSprite;
        }

        foldoutSize = foldoutSprite.bounds.size;
    }

    [ContextMenu("Set Colors")]
    void SetFoldOutColors()
    {
        for (int i = 0; i < foldOutSprites.Count; i++)
        {
            foldOutSprites[i].color = playerData.ColorSet[i];
        }
    }



    public void FoldoutLeftStart(InputAction.CallbackContext inputCallback)
    {

    }

    public void FoldoutRightStart(InputAction.CallbackContext inputCallback)
    {
        if (m_foldoutSequence != null)
        {
            if (m_foldoutSequence.IsPlaying())
            {
                m_foldoutSequence.PlayForward();
                return;
            }
            else
                m_foldoutSequence.Kill();
        }

        playerMovement.enabled = false;

        m_foldoutSequence = DOTween.Sequence().Pause().SetAutoKill(false);

        m_foldoutSequence.OnRewind(() => OnFoldOutReset());
        m_foldoutSequence.OnKill(() => OnFoldOutKilled());
        m_foldoutSequence.OnComplete(() => groundCheck.transform.position = foldOutTransforms[playerData.GroupCount - 1].transform.position);

        m_foldoutOffset.x = foldoutSize.x / 2;

        m_foldoutPosition = Vector2.zero;
        m_foldoutPosition.x -= m_foldoutOffset.x;

        m_foldoutScale.x = 0;
        m_foldoutScale.y = 1;


        for (int i = 0; i < playerData.GroupCount; i++)
        {
            m_foldoutPosition.x += foldoutSize.x;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = m_foldoutOffset;

            foldOutTransforms[i].gameObject.SetActive(true);

            foldOutTransforms[i].localScale = m_foldoutScale;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(1, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(cameraTarget.DOMoveX(foldOutSprites[i].transform.position.x, m_foldOutTime).SetEase(Ease.OutExpo));
        }



        m_foldoutSequence.Play();
    }

    public void FoldoutUpStart(InputAction.CallbackContext inputCallback)
    {

    }

    public void FoldoutUpEnd(InputAction.CallbackContext inputCallback)
    {

    }

    public void FoldoutHorizontalEnd(InputAction.CallbackContext inputCallback)
    {
        if (m_foldoutSequence.IsComplete())
        {
            if (groundCheck.IsGrounded)
            {

                _succesfullFoldout = true;
                m_foldoutSequence.Kill();
                SuccessfulHorizontalFoldOut();
                return;
            }
        }

        m_foldoutSequence.PlayBackwards();
    }

    void SuccessfulHorizontalFoldOut()
    {
        m_foldoutSequence = DOTween.Sequence().Pause();

        for (int i = 0; i < playerData.GroupCount; i++)
        {
            m_foldoutPosition = foldOutTransforms[i].localPosition;
            m_foldoutPosition.x += foldoutSize.x;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = -m_foldoutOffset;
        }

        for (int i = 0; i < playerData.GroupCount; i++)
        {

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(0, m_foldOutTime).SetEase(Ease.InExpo));
            m_foldoutSequence.Join(playerSpriteObject.DOMoveX(foldOutSprites[i].transform.position.x, m_foldOutTime).SetEase(Ease.InExpo));

        }

        m_foldoutSequence.Play().OnComplete(() => OnSuccessfulFoldOut());
    }

    void OnSuccessfulFoldOut()
    {
        playerMovement.transform.position = foldOutSprites[playerData.GroupCount - 1].transform.position;

        
        playerSpriteObject.transform.localPosition = Vector3.zero;
        cameraTarget.transform.localPosition = Vector3.zero;
        groundCheck.transform.localPosition = Vector3.zero;

        OnFoldOutReset();
    }


    void OnFoldOutReset()
    {
        m_foldoutSequence.Kill();
        playerMovement.enabled = true;
    }

    void OnFoldOutKilled()
    {
        if (!_succesfullFoldout)
        {
            m_foldoutPosition = Vector2.zero;
            for (int i = 0; i < playerData.GroupCount; i++)
            {
                foldOutTransforms[i].localPosition = m_foldoutPosition;
                foldOutTransforms[i].gameObject.SetActive(false);
            }
        }
        else
        {
            _succesfullFoldout = false;
        }
    }
}
