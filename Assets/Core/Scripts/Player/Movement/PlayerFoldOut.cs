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
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] PlayerGroundCheck2D playerGroundCheck;
    [SerializeField] PlayerGroundCheck2D foldoutGroundCheck;
    [SerializeField] PlayerLedgeCheck2D ledgeCheck;
    [SerializeField] FoldOutAnimator animator;
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform playerSpriteObject;
    [SerializeField] PlayerAudio audioPlayer;
    [SerializeField] AudioClip foldSFX;
    [SerializeField] AudioClip moveSFX;

    [Space()]
    [SerializeField] List<Transform> foldOutTransforms;
    [SerializeField] List<SpriteRenderer> foldOutSprites;

    [Header("Settings")]
    [SerializeField] float foldoutSpeed = 1;
    [SerializeField] float foldoutAcceleration = 1;
    [SerializeField] float climbHopDistance = 1;
    [SerializeField] Vector2 foldoutSize;

    Vector2 m_foldoutPosition;
    Vector2 m_foldoutOffset;
    Vector3 m_foldoutScale = Vector3.one;

    Sequence m_foldoutSequence;

    float m_foldOutTime;

    int m_maxFoldOuts = 0;
    public int m_endIndex = 0;

    System.Action m_counter;

    enum Direction
    {
        UP = 0,
        RIGHT = 1,
        LEFT = -1
    }
    Direction m_direction;

    enum FoldoutState
    {
        IDLE,
        OUT,
        IN,
        MOVE,
        INTERRUPT
    }
    FoldoutState m_currentState;

    public void FoldoutLeftStart(InputAction.CallbackContext inputCallback)
    {
        if (m_currentState == FoldoutState.IDLE || m_direction == Direction.LEFT)
        {
            m_direction = Direction.LEFT;
            FoldoutHorizontalStart();
        }
    }

    public void FoldoutRightStart(InputAction.CallbackContext inputCallback)
    {
        if (m_currentState == FoldoutState.IDLE || m_direction == Direction.RIGHT)
        {
            m_direction = Direction.RIGHT;
            FoldoutHorizontalStart();
        }
    }

    void FoldoutHorizontalStart()
    {
        if(!playerGroundCheck.IsGrounded)
            return;

        if (m_currentState == FoldoutState.INTERRUPT || m_currentState == FoldoutState.MOVE)
            return;

        if (m_currentState == FoldoutState.IN)
        {
            FoldOutForward();
            return;
        }


        if (playerData.AlternateControls)
        {
            if (m_currentState == FoldoutState.OUT)
            {
                FoldOutInward();
                return;
            }
        }

        playerMovement.enabled = false;

        InitalizeSequence();

        m_maxFoldOuts = playerData.GroupCount;//Mathf.Min(CheckHorizontalDistance(), playerData.GroupCount);
        m_foldoutOffset.y = 0;
        m_foldoutOffset.x = foldoutSize.x / 2 * (int)m_direction;

        m_foldoutPosition = Vector2.zero;

        m_foldoutPosition.x -= m_foldoutOffset.x;
        m_foldoutScale.x = 0;
        m_foldoutScale.y = 1;

        float temp;

        for (int i = 0; i < m_maxFoldOuts; i++)
        {
            m_foldoutPosition.x += foldoutSize.x * (int)m_direction;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = m_foldoutOffset;

            temp = foldOutSprites[i].transform.position.x;

            foldOutTransforms[i].gameObject.SetActive(true);

            foldOutTransforms[i].localScale = m_foldoutScale;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.AppendCallback(() => CheckDistance());
            m_foldoutSequence.AppendCallback(() => audioPlayer.PlayClipSoft(foldSFX));
            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(1, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(cameraTarget.DOMoveX(temp, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.AppendCallback(() => m_counter());

            //if (playerData.AlternateControls)
            //m_foldoutSequence.AppendCallback(() => FoldoutEnd(foldoutGroundCheck.IsGrounded));


            m_foldoutSequence.AppendCallback(() => CheckForInterruption());
        }

        if (playerData.AlternateControls)
            m_foldoutSequence.OnComplete(() => FoldoutEnd(foldoutGroundCheck.IsGrounded));

        animator.SetIndex();
        animator.SetDirection(false, true);

        FoldOutForward();


    }

    public void FoldoutRightEnd(InputAction.CallbackContext inputCallback)
    {
        if (playerData.AlternateControls) return;

        if (m_currentState == FoldoutState.OUT && m_direction == Direction.RIGHT)
        {
            FoldoutEnd(foldoutGroundCheck.IsGrounded);
        }
    }

    public void FoldoutLeftEnd(InputAction.CallbackContext inputCallback)
    {
        if (playerData.AlternateControls) return;

        if (m_currentState == FoldoutState.OUT && m_direction == Direction.LEFT)
        {
            FoldoutEnd(foldoutGroundCheck.IsGrounded);
        }
    }

    void MoveFoldOutHorizontal()
    {
        m_currentState = FoldoutState.MOVE;

        InitalizeSequence();

        for (int i = 0; i < m_endIndex; i++)
        {
            m_foldoutPosition = foldOutTransforms[i].localPosition;
            m_foldoutPosition.x += foldoutSize.x * (int)m_direction;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = -m_foldoutOffset;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.AppendCallback(() => audioPlayer.PlayClipSoft(moveSFX));
            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(0, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(playerSpriteObject.DOMoveX(foldOutSprites[i].transform.position.x, m_foldOutTime).SetEase(Ease.OutExpo));
        }

        playerBody.isKinematic = true;
        m_foldoutSequence.Play().OnComplete(() => PostMoveHorizontal());
    }

    void PostMoveHorizontal()
    {
        playerMovement.transform.position = playerSpriteObject.transform.position;

        playerSpriteObject.transform.localPosition = Vector3.zero;
        cameraTarget.transform.localPosition = Vector3.zero;

        OnFoldOutReset();
        OnFoldOutKilled();
    }

    #region Vertical Movement

    public void FoldoutUpStart(InputAction.CallbackContext inputCallback)
    {
        if(!playerGroundCheck.IsGrounded)
            return;

        if (!(m_currentState == FoldoutState.IDLE || m_direction == Direction.UP))
            return;

        if (m_currentState == FoldoutState.INTERRUPT || m_currentState == FoldoutState.MOVE)
            return;

        m_direction = Direction.UP;

        if (m_currentState == FoldoutState.IN)
        {
            FoldOutForward();
            return;
        }

        if (playerData.AlternateControls)
        {
            if (m_currentState == FoldoutState.OUT)
            {
                FoldOutInward();
                return;
            }
        }

        playerMovement.enabled = false;

        InitalizeSequence();

        m_maxFoldOuts = playerData.GroupCount;//Mathf.Min(CheckVerticalDistance(), playerData.GroupCount);

        m_foldoutOffset.x = 0;
        m_foldoutOffset.y = foldoutSize.y / 2;

        m_foldoutPosition = Vector2.zero;

        m_foldoutPosition.y -= m_foldoutOffset.y;
        m_foldoutScale.x = 1;
        m_foldoutScale.y = 0;

        float temp;

        for (int i = 0; i < m_maxFoldOuts; i++)
        {
            m_foldoutPosition.y += foldoutSize.y;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = m_foldoutOffset;

            foldOutTransforms[i].gameObject.SetActive(true);

            temp = foldOutSprites[i].transform.position.y;

            foldOutTransforms[i].localScale = m_foldoutScale;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.AppendCallback(() => CheckDistance());
            m_foldoutSequence.AppendCallback(() => audioPlayer.PlayClipSoft(foldSFX));
            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleY(1, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(cameraTarget.DOMoveY(temp, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.AppendCallback(() => m_counter());

            if (playerData.AlternateControls && i < m_maxFoldOuts - 1)
                m_foldoutSequence.AppendCallback(() =>
                {
                    if (ledgeCheck.CheckLedge()) m_currentState = FoldoutState.INTERRUPT;
                }
                    );

            m_foldoutSequence.AppendCallback(() => CheckForInterruption());
        }

        if (playerData.AlternateControls)
            m_foldoutSequence.OnComplete(() => FoldoutEnd(ledgeCheck.CheckLedge()));

        animator.SetIndex();
        animator.SetDirection(true, false);

        FoldOutForward();
    }

    public void FoldoutUpEnd(InputAction.CallbackContext inputCallback)
    {
        if (playerData.AlternateControls) return;

        if (m_currentState == FoldoutState.OUT && m_direction == Direction.UP)
        {
            FoldoutEnd(ledgeCheck.CheckLedge());
        }
    }

    void MoveFoldOutVertical()
    {
        m_currentState = FoldoutState.MOVE;

        InitalizeSequence();

        for (int i = 0; i < m_endIndex; i++)
        {
            m_foldoutPosition = foldOutTransforms[i].localPosition;
            m_foldoutPosition.y += foldoutSize.y;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = -m_foldoutOffset;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.AppendCallback(() => audioPlayer.PlayClipSoft(moveSFX));
            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleY(0, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(playerSpriteObject.DOMoveY(foldOutSprites[i].transform.position.y, m_foldOutTime).SetEase(Ease.OutExpo));
        }

        playerBody.isKinematic = true;
        m_foldoutSequence.Play().OnComplete(() => PostMoveVertical());
    }

    void PostMoveVertical()
    {
        playerMovement.transform.position = playerSpriteObject.transform.position;

        playerSpriteObject.transform.localPosition = Vector3.zero;
        cameraTarget.transform.localPosition = Vector3.zero;

        Vector2 newPlayerPos = ledgeCheck.CornerPosition;
        newPlayerPos.y += foldoutSize.y / 2 * transform.localScale.x;

        float xDist = newPlayerPos.x - playerMovement.transform.position.x;
        float yDist = newPlayerPos.y - playerMovement.transform.position.y;

        DOTween.Sequence()
        .Append(playerMovement.transform.DOBlendableMoveBy(new Vector3(0, yDist + climbHopDistance, 0), 0.5f))
        .Insert(0.2f, playerMovement.transform.DOBlendableMoveBy(new Vector3(xDist + 0.5f * Mathf.Sign(xDist), 0, 0), 0.5f))
        .OnComplete(() =>
        {
            OnFoldOutReset();
            OnFoldOutKilled();
        });
    }

    #endregion

    void FixedUpdate() 
    {
        if(!playerGroundCheck.IsGrounded)
        {
            if(m_currentState == FoldoutState.OUT)
                FoldOutInward();
        }
    }


    #region Helper Methods

    void InitalizeSequence()
    {
        m_foldoutSequence = DOTween.Sequence().Pause().SetAutoKill(false);

        m_foldoutSequence.OnRewind(() => OnFoldOutReset());
        m_foldoutSequence.OnKill(() => OnFoldOutKilled());
    }

    void FoldOutForward()
    {
        m_currentState = FoldoutState.OUT;
        m_counter = () => m_endIndex++;
        m_foldoutSequence.PlayForward();
    }

    void FoldOutInward()
    {
        m_currentState = FoldoutState.IN;
        m_counter = () => m_endIndex--;
        m_foldoutSequence.PlayBackwards();
    }

    void FoldoutEnd(bool condition)
    {
        if (condition)
        {
            m_currentState = FoldoutState.INTERRUPT;

            if (m_foldoutSequence.IsComplete())
                CheckForInterruption();

            return;
        }

        FoldOutInward();
    }

    void CheckDistance()
    {
        

        if (m_currentState != FoldoutState.OUT)
            return;

        if (m_direction == Direction.UP)
        {
            if (CheckVerticalDistance())
            {
                if (m_endIndex != 0)
                {
                    FoldoutEnd(ledgeCheck.CheckLedge());
                    CheckForInterruption();
                }
                else
                    FoldOutInward();

            }
        }
        else
        {
            if (CheckHorizontalDistance())
            {
                if (m_endIndex != 0)
                {
                    FoldoutEnd(foldoutGroundCheck.IsGrounded);
                    CheckForInterruption();
                }

                else
                    FoldOutInward();
            }
        }

    }

    void CheckForInterruption()
    {
        if (m_currentState != FoldoutState.INTERRUPT)
            return;

        m_currentState = FoldoutState.MOVE;

        m_foldoutSequence.Kill();

        if (m_direction == Direction.UP)
            MoveFoldOutVertical();
        else
            MoveFoldOutHorizontal();
    }

    void OnFoldOutReset()
    {
        animator.SetDirection();

        playerBody.isKinematic = false;
        playerMovement.enabled = true;
        m_endIndex = 0;

        m_currentState = FoldoutState.IDLE;
        m_foldoutSequence.Kill();
    }

    void OnFoldOutKilled()
    {
        m_foldoutSequence = null;

        if (m_currentState != FoldoutState.MOVE)
        {
            m_foldoutPosition = Vector2.zero;
            for (int i = 0; i < playerData.GroupCount; i++)
            {
                foldOutTransforms[i].localPosition = m_foldoutPosition;
                foldOutTransforms[i].gameObject.SetActive(false);
                foldOutTransforms[i].localScale = Vector2.one;
            }

            m_currentState = FoldoutState.IDLE;
        }
    }

    //UNOPTIMIZED
    bool CheckHorizontalDistance()
    {
        Vector3 origin = foldOutTransforms[m_endIndex].position;

        RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.right * (int)m_direction, foldoutSize.x * transform.localScale.x * 1.2f, playerData.LevelMask);
        
        return raycastHit.collider;

        //if (raycastHit.collider)
        //return Mathf.FloorToInt(raycastHit.distance / (foldoutSize.x * transform.localScale.x));

        // return playerData.GroupCount;
    }

    //UNOPTIMIZED
    bool CheckVerticalDistance()
    {
        Vector3 origin = m_endIndex == 0 ? transform.position : foldOutSprites[m_endIndex - 1].transform.position;

        RaycastHit2D raycastHit = Physics2D.Raycast(origin + new Vector3(0, (foldoutSize.y * transform.localScale.x) / 2, 0), Vector2.up, foldoutSize.y * transform.localScale.x, playerData.LevelMask);

        return raycastHit.collider;

        //if (raycastHit.collider)
        // return Mathf.FloorToInt(raycastHit.distance / (foldoutSize.y * transform.localScale.x));

        // return playerData.GroupCount;
    }

    #endregion


    #region Editor Helper Methods


    [ContextMenu("Set Colors")]
    void SetFoldOutColors()
    {
        for (int i = 0; i < foldOutSprites.Count; i++)
        {
            foldOutSprites[i].color = playerData.ColorSet[i];
        }
    }

    void ReverseFoldOutColors()
    {
        for (int i = foldOutSprites.Count - 1; i >= 0; i--)
        {
            foldOutSprites[i].color = playerData.ColorSet[i];
        }
    }
    #endregion
}
