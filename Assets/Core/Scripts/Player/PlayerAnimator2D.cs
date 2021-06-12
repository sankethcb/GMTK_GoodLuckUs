using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] bool flipSprite = false;

    Vector2 m_spriteDirection;

    public void CheckDirection(InputAction.CallbackContext inputCallback)
    {
        m_spriteDirection = inputCallback.ReadValue<Vector2>();

        if (m_spriteDirection.x > 0)
        {
            spriteRenderer.flipX = flipSprite;
        }
        else if (m_spriteDirection.x < 0)
        {
            spriteRenderer.flipX = !flipSprite;
        }
    }


}
