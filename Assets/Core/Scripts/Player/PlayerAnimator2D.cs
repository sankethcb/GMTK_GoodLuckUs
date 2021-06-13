using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerGroundCheck2D groundCheck;


    Vector2 m_spriteDirection;

    bool m_walkingRight = false;
    bool m_walkingLeft = false;

    public void CheckMoveDirection(InputAction.CallbackContext inputCallback)
    {
        m_spriteDirection = inputCallback.ReadValue<Vector2>();

        if (m_spriteDirection.x > 0)
        {
            if (!m_walkingRight)
            {
                m_walkingRight = true;
                animator.SetBool("WalkRight", m_walkingRight);
            }

            if (m_walkingLeft)
            {
                m_walkingLeft = false;
                animator.SetBool("WalkLeft", m_walkingLeft);
            }
        }
        else if (m_spriteDirection.x < 0)
        {
            if (!m_walkingLeft)
            {
                m_walkingLeft = true;
                animator.SetBool("WalkLeft", m_walkingLeft);
            }

            if (m_walkingRight)
            {
                m_walkingRight = false;
                animator.SetBool("WalkRight", m_walkingRight);
            }
        }
    }

    public void StopMove(InputAction.CallbackContext inputCallback) => Stop();

    void FixedUpdate()
    {
        if (!groundCheck.IsGrounded) Stop();
    }

    void Stop()
    {
        if (m_walkingLeft)
        {
            m_walkingLeft = false;
            animator.SetBool("WalkLeft", m_walkingLeft);
        }

        if (m_walkingRight)
        {
            m_walkingRight = false;
            animator.SetBool("WalkRight", m_walkingRight);
        }
    }

    public void HighFive(string dir) => animator.SetTrigger("Highfive" + dir);

    public void Wave()
    {
        Stop();
        animator.SetTrigger("Wave");
    }

}
