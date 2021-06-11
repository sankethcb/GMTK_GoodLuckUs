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
    [SerializeField] float _playerSpeed = 1;
    [SerializeField] bool _allowAirMovement = true;


    Vector2 m_playerDirection;
    Vector2 m_playerVelocity;
    Vector2 m_playerMovement;


    void Awake()
    {
        m_playerVelocity = Vector2.zero;
        m_playerDirection = Vector2.zero;
        m_playerMovement = Vector2.zero;
    }

    public void MovePlayer(InputAction.CallbackContext inputCallback)
    {
        m_playerDirection = inputCallback.ReadValue<Vector2>();

        m_playerDirection.y = 0;

        m_playerMovement = m_playerDirection * _playerSpeed;
    }


    public void StopPlayer(InputAction.CallbackContext inputCallback)
    {
        m_playerMovement = Vector2.zero;
    }


    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        m_playerVelocity = playerBody.velocity;

        if (!_allowAirMovement)
        {
            if (!groundCheck.IsGrounded)
                return;
        }
        m_playerVelocity.x = m_playerMovement.x;

        playerBody.velocity = m_playerVelocity;
    }
}
