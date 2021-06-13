using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;

    [Header("Settings")]
    [SerializeField] float groundRaycastDist = 0.1f;
    [SerializeField] LayerMask levelMask;

    [Header ("Variables")]
    [SerializeField] bool _isGrounded = false;

    public bool IsGrounded => _isGrounded;

    RaycastHit2D m_raycastLeftFoot;
    RaycastHit2D m_raycastRightFoot;


    void FixedUpdate() 
    {
        CheckGrounding();
    }

    public void CheckGrounding()
    {
        m_raycastLeftFoot = Physics2D.Raycast(leftFoot.position, Vector2.down, groundRaycastDist, levelMask);
        m_raycastRightFoot = Physics2D.Raycast(rightFoot.position, Vector2.down, groundRaycastDist, levelMask);

        _isGrounded =  (m_raycastLeftFoot.collider || m_raycastRightFoot.collider);
    }
}
