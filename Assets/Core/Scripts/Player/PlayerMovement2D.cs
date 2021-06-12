using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] PlayerGroundCheck2D groundCheck;

    [Header("Settings")]
    [SerializeField] float playerSpeed = 10;
    [SerializeField] bool allowAirMovement = true;
    [Range(0, .3f)] [SerializeField] float smoothing = .05f;	


    Vector2 m_playerDirection;
    Vector2 m_playerVelocityTarget;
    Vector3 m_playerVelocityCurrent;

    void Awake()
    {
        m_playerVelocityTarget = Vector2.zero;
        m_playerVelocityCurrent = Vector2.zero;

        m_playerDirection = Vector2.zero;
    }

    public void MovePlayer(InputAction.CallbackContext inputCallback)
    {
        m_playerDirection = inputCallback.ReadValue<Vector2>();

        m_playerDirection.y = 0;

        m_playerVelocityTarget.x = m_playerDirection.x * playerSpeed * Time.fixedDeltaTime;
    }

    public void StopPlayer(InputAction.CallbackContext inputCallback)
    {
        m_playerVelocityTarget = Vector2.zero;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!(groundCheck.IsGrounded || allowAirMovement))
            return;

        m_playerVelocityTarget.y = playerBody.velocity.y;

        playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, m_playerVelocityTarget, ref m_playerVelocityCurrent, smoothing);
        
    }
}
