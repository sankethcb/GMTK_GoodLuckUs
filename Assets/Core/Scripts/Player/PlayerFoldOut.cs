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
    [SerializeField] PlayerLedgeCheck2D ledgeCheck;
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform playerSpriteObject;

    [Space()]
    [SerializeField] List<Transform> foldOutTransforms;
    [SerializeField] List<SpriteRenderer> foldOutSprites;

    [Header("Settings")]
    [SerializeField] Sprite foldoutSprite;
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
    int m_endIndex = 0;

    System.Action m_counter;

    enum Direction
    {
        UP = 0,
        RIGHT = 1,
        LEFT = -1
    }
    [SerializeField] Direction direction;

    enum FoldoutState
    {
        IDLE,
        OUT,
        IN,
        MOVE,
        INTERRUPT
    }
    [SerializeField] FoldoutState currentState;

    public void FoldoutLeftStart(InputAction.CallbackContext inputCallback)
    {
        if (currentState == FoldoutState.IDLE || direction == Direction.LEFT)
        {
            direction = Direction.LEFT;
            FoldoutHorizontalStart();
        }

    }

    public void FoldoutRightStart(InputAction.CallbackContext inputCallback)
    {

        if (currentState == FoldoutState.IDLE || direction == Direction.RIGHT)
        {
            direction = Direction.RIGHT;
            FoldoutHorizontalStart();
        }
    }

    void FoldoutHorizontalStart()
    {
        if (currentState == FoldoutState.INTERRUPT || currentState == FoldoutState.MOVE)
            return;

    
        if (currentState == FoldoutState.IN)
        {
            FoldOutForward();
            return;
        }

        playerMovement.enabled = false;

        InitalizeSequence();

        m_maxFoldOuts = Mathf.Min(CheckHorizontalDistance(), playerData.GroupCount);
        m_foldoutOffset.y = 0;
        m_foldoutOffset.x = foldoutSize.x / 2 * (int)direction;

        m_foldoutPosition = Vector2.zero;

        m_foldoutPosition.x -= m_foldoutOffset.x;
        m_foldoutScale.x = 0;
        m_foldoutScale.y = 1;

        float temp;

        for (int i = 0; i < m_maxFoldOuts; i++)
        {
            m_foldoutPosition.x += foldoutSize.x * (int)direction;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = m_foldoutOffset;

            temp = foldOutSprites[i].transform.position.x;

            foldOutTransforms[i].gameObject.SetActive(true);

            foldOutTransforms[i].localScale = m_foldoutScale;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(1, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(cameraTarget.DOMoveX(temp, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.AppendCallback(() => m_counter());
            m_foldoutSequence.AppendCallback(() => CheckForInterruption());
        }

        FoldOutForward();
    }

    public void FoldoutRightEnd(InputAction.CallbackContext inputCallback)
    {
        if (currentState == FoldoutState.OUT && direction == Direction.RIGHT)
        {
            FoldoutEnd(groundCheck.IsGrounded);
        }
    }

    public void FoldoutLeftEnd(InputAction.CallbackContext inputCallback)
    {
        if (currentState == FoldoutState.OUT && direction == Direction.LEFT)
        {
            FoldoutEnd(groundCheck.IsGrounded);
        }
    }

    void MoveFoldOutHorizontal()
    {
        currentState = FoldoutState.MOVE;

        InitalizeSequence();

        for (int i = 0; i < m_endIndex; i++)
        {
            m_foldoutPosition = foldOutTransforms[i].localPosition;
            m_foldoutPosition.x += foldoutSize.x * (int)direction;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = -m_foldoutOffset;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleX(0, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(playerSpriteObject.DOMoveX(foldOutSprites[i].transform.position.x, m_foldOutTime).SetEase(Ease.OutExpo));
        }

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

    public void FoldoutUpStart(InputAction.CallbackContext inputCallback)
    {
        if (!(currentState == FoldoutState.IDLE || direction == Direction.UP))
            return;

        if (currentState == FoldoutState.INTERRUPT || currentState == FoldoutState.MOVE)
            return;

        direction = Direction.UP;

        if (currentState == FoldoutState.IN)
        {
            FoldOutForward();
            return;
        }

        playerMovement.enabled = false;

        InitalizeSequence();

        m_maxFoldOuts = Mathf.Min(CheckVerticalDistance(), playerData.GroupCount);

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

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleY(1, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(cameraTarget.DOMoveY(temp, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.AppendCallback(() => m_counter());
            m_foldoutSequence.AppendCallback(() => CheckForInterruption());
        }

        FoldOutForward();
    }

    public void FoldoutUpEnd(InputAction.CallbackContext inputCallback)
    {
        if (currentState == FoldoutState.OUT && direction == Direction.UP)
        {
            FoldoutEnd(ledgeCheck.CheckLedge());
        }
    }

    void MoveFoldOutVertical()
    {
        currentState = FoldoutState.MOVE;

        InitalizeSequence();

        for (int i = 0; i < m_endIndex; i++)
        {
            m_foldoutPosition = foldOutTransforms[i].localPosition;
            m_foldoutPosition.y += foldoutSize.y;

            foldOutTransforms[i].localPosition = m_foldoutPosition;
            foldOutSprites[i].transform.localPosition = -m_foldoutOffset;

            m_foldOutTime = 1 / (foldoutSpeed + foldoutAcceleration * i);

            m_foldoutSequence.Append(foldOutTransforms[i].DOScaleY(0, m_foldOutTime).SetEase(Ease.OutExpo));
            m_foldoutSequence.Join(playerSpriteObject.DOMoveY(foldOutSprites[i].transform.position.y, m_foldOutTime).SetEase(Ease.OutExpo));
        }

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
        .Insert(0.2f, playerMovement.transform.DOBlendableMoveBy(new Vector3(xDist + 0.5f * Mathf.Sign(xDist), 0, 0), 0.5f));

        OnFoldOutReset();
        OnFoldOutKilled();
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
        currentState = FoldoutState.OUT;
        m_counter = () => m_endIndex++;
        m_foldoutSequence.PlayForward();
    }

    void FoldOutInward()
    {
        currentState = FoldoutState.IN;
        m_counter = () => m_endIndex--;
        m_foldoutSequence.PlayBackwards();
    }

    void FoldoutEnd(bool condition)
    {
        if (condition)
        {
            currentState = FoldoutState.INTERRUPT;

            if (m_foldoutSequence.IsComplete())
                CheckForInterruption();

            return;
        }

        FoldOutInward();
    }

    void CheckForInterruption()
    {
        if (currentState != FoldoutState.INTERRUPT)
            return;

        currentState = FoldoutState.MOVE;

        m_foldoutSequence.Kill();

        if (direction == Direction.UP)
            MoveFoldOutVertical();
        else
            MoveFoldOutHorizontal();
    }

    void OnFoldOutReset()
    {
        playerMovement.enabled = true;
        m_endIndex = 0;

        currentState = FoldoutState.IDLE;
        m_foldoutSequence.Kill();

    }

    void OnFoldOutKilled()
    {
        m_foldoutSequence = null;

        if (currentState != FoldoutState.MOVE)
        {
            m_foldoutPosition = Vector2.zero;
            for (int i = 0; i < m_maxFoldOuts; i++)
            {
                foldOutTransforms[i].localPosition = m_foldoutPosition;
                foldOutTransforms[i].gameObject.SetActive(false);
                foldOutTransforms[i].localScale = Vector2.one;
            }

            currentState = FoldoutState.IDLE;
        }
    }

    int CheckHorizontalDistance()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + new Vector3((foldoutSize.x * transform.localScale.x) / 2, 0, 0) * (int)direction, Vector2.right * (int)direction, playerData.GroupCount * foldoutSize.x * transform.localScale.x, playerData.LevelMask);

        if (raycastHit.collider)
            return Mathf.FloorToInt(raycastHit.distance / (foldoutSize.x * transform.localScale.x));

        return playerData.GroupCount;
    }

    int CheckVerticalDistance()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + new Vector3(0, (foldoutSize.y * transform.localScale.x) / 2, 0), Vector2.up, playerData.GroupCount * foldoutSize.y * transform.localScale.x, playerData.LevelMask);

        if (raycastHit.collider)
            return Mathf.FloorToInt(raycastHit.distance / (foldoutSize.y * transform.localScale.x));

        return playerData.GroupCount;
    }
    #endregion


    #region Editor Helper Methods
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

    void ReverseFoldOutColors()
    {
        for (int i = foldOutSprites.Count - 1; i >= 0; i--)
        {
            foldOutSprites[i].color = playerData.ColorSet[i];
        }
    }
    #endregion
}
