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

    RaycastHit2D _raycastLeftFoot;
    RaycastHit2D _raycastRightFoot;

    int _levelMask;

    void Awake()
    {
        _levelMask = LayerMask.GetMask("Level");
    }

    public bool IsGrounded()
    {
        _raycastLeftFoot = Physics2D.Raycast(leftFoot.position, Vector2.down, groundRaycastDist, _levelMask);
        _raycastRightFoot = Physics2D.Raycast(rightFoot.position, Vector2.down, groundRaycastDist, _levelMask);

        return (_raycastLeftFoot.collider || _raycastRightFoot.collider);
    }
}
