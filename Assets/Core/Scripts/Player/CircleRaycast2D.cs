using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRaycast2D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float circleRaycastRadius = 1.0f;
    [SerializeField] LayerMask levelMask;

    RaycastHit2D m_raycastHit;
    public RaycastHit2D Hit2D => m_raycastHit;

    public bool CheckCircle()
    {
        m_raycastHit = Physics2D.CircleCast(transform.position, circleRaycastRadius, Vector2.zero, 0, levelMask);
        return m_raycastHit.collider;
    }
}
