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


    void Awake()
    {
        m_playerVelocity = Vector2.zero;
        m_playerDirection = Vector2.zero;
    }

    public void MovePlayer(InputAction.CallbackContext inputCallback)
    {
        if(!_allowAirMovement)
        {
            if(!groundCheck.IsGrounded())
                return;
        }

        m_playerDirection = inputCallback.ReadValue<Vector2>();

        m_playerDirection.y = 0;

        m_playerVelocity = m_playerDirection * _playerSpeed;

        playerBody.velocity = m_playerVelocity;
    }


    public void StopPlayer(InputAction.CallbackContext inputCallback)
    {
        m_playerVelocity = Vector2.zero;
        playerBody.velocity = m_playerVelocity;
    }
}
